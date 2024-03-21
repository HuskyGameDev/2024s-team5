using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nc543.Nav2D{
    public class NavigationGrid : MonoBehaviour{
        public LayerMask blockingMask;
        public Vector2 gridSize;
        public float nodeSize;
        public TerrainType[] walkableRegions;
        LayerMask walkableMask;
        Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

        [Header("Pathing Settings")]
        public int obstacleAvoidanceStrength = 10;
        public bool smoothWeights = true;
        public int smoothing = 1;

        [Header("Debug")]
        public bool collisionDebug = false;
        public bool onlyDisplayPath = false;
        public List<Node> path;

        int penaltyMin = int.MaxValue;
        int penaltyMax = int.MinValue;

        Node[,] navGrid;
        int nodesX;
        int nodesY;

        void Awake(){
            nodesX = Mathf.RoundToInt(gridSize.x/nodeSize);
            nodesY = Mathf.RoundToInt(gridSize.y/nodeSize);

            foreach (TerrainType region in walkableRegions){
                walkableMask.value = walkableMask | region.terrainMask.value;
                walkableRegionsDictionary.Add((int) Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
                if (region.terrainPenalty > penaltyMax) penaltyMax = region.terrainPenalty;
                if (region.terrainPenalty < penaltyMin) penaltyMin = region.terrainPenalty;
            }

            generateNavigationGrid();
        }

        private void generateNavigationGrid(){
            navGrid = new Node[nodesX, nodesY];

            Vector2 bottomLeftLoc = new Vector2(transform.position.x, transform.position.y) - (Vector2.right * (gridSize.x / 2)) - (Vector2.up * (gridSize.y / 2));

            for (int i = 0; i < nodesX; i++){
                for (int j = 0; j < nodesY; j++){
                    Vector2 loc = bottomLeftLoc + (Vector2.right * ((i * nodeSize) + (nodeSize / 2))) + (Vector2.up * ((j * nodeSize) + (nodeSize / 2)));
                    bool traverse = !Physics2D.OverlapBox(loc, new Vector2(nodeSize / 2, nodeSize / 2), 0, blockingMask);
                    int movementPenalty = 0;
                    Collider2D spot = Physics2D.OverlapBox(loc, new Vector2(nodeSize / 2, nodeSize / 2), 0, walkableMask);
                    if (traverse && spot){
                        walkableRegionsDictionary.TryGetValue(spot.gameObject.layer, out movementPenalty);
                    }else if (!traverse){
                        movementPenalty += obstacleAvoidanceStrength;
                    }
                    navGrid[i, j] = new Node(traverse, loc, i, j, movementPenalty);
                }
            }
            if (smoothWeights) blurMap(smoothing);
        }

        void blurMap(int blurSize){
            int kernelSize = blurSize * 2 + 1;

            int[,] horizontalPass = new int[nodesX, nodesY];
            int[,] verticalPass = new int[nodesX, nodesY];

            for (int i = 0; i < nodesY; i++){
                for (int j = -blurSize; j <= blurSize; j++){
                    int sampleX = Mathf.Clamp(j, 0, blurSize);
                    horizontalPass[0, i] += navGrid[sampleX, i].movementPenalty;
                }

                for (int j = 1; j < nodesX; j++){
                    int removeIndex = Mathf.Clamp(j - blurSize - 1, 0, nodesX);
                    int addIndex = Mathf.Clamp(j + blurSize, 0, nodesX - 1);
                    horizontalPass[j, i] = horizontalPass[j - 1, i] - navGrid[removeIndex, i].movementPenalty + navGrid[addIndex, i].movementPenalty;
                }
            }

            for (int i = 0; i < nodesX; i++){
                for (int j = -blurSize; j <= blurSize; j++){
                    int sampleY = Mathf.Clamp(j, 0, blurSize);
                    verticalPass[i, 0] += horizontalPass[i, sampleY];
                }

                int blurredPenalty = Mathf.RoundToInt((float) verticalPass[i, 0] / (kernelSize * kernelSize));
                navGrid[i, 0].movementPenalty = blurredPenalty;

                for (int j = 1; j < nodesY; j++){
                    int removeIndex = Mathf.Clamp(j - blurSize - 1, 0, nodesY);
                    int addIndex = Mathf.Clamp(j + blurSize, 0, nodesY - 1);
                    verticalPass[i, j] = verticalPass[i, j - 1] - horizontalPass[i, removeIndex] + horizontalPass[i, addIndex];
                    blurredPenalty = Mathf.RoundToInt((float) verticalPass[i, j] / (kernelSize * kernelSize));
                    navGrid[i, j].movementPenalty = blurredPenalty;

                    if (blurredPenalty > penaltyMax){
                        penaltyMax = blurredPenalty;
                    }else if (blurredPenalty < penaltyMin){
                        penaltyMin = blurredPenalty;
                    }
                }
            }
        }

        public List<Node> getNeighbors(Node node){
            List<Node> neighbors = new List<Node>();
            for (int i = -1; i <= 1; i++){
                for (int j = -1; j <= 1; j++){
                    if (i == 0 && j == 0) continue;
                    int x = node.x + i;
                    int y = node.y + j;

                    if (x >= 0 && x < nodesX && y >= 0 && y < nodesY){
                        neighbors.Add(navGrid[x, y]);
                    }
                }
            }

            return neighbors;
        }

        public Node worldToNavGrid(Vector2 pos){
            float percentX = Mathf.Clamp01(((pos.x * 1.025f - transform.position.x) / gridSize.x) + 0.5f);
            float percentY = Mathf.Clamp01(((pos.y * 1.025f - transform.position.y) / gridSize.y) + 0.5f);

            return navGrid[Mathf.RoundToInt((nodesX - 1) * percentX), Mathf.RoundToInt((nodesY - 1) * percentY)];
        }

        public void findPath(PathRequest request, Action<PathResult> callback){
            Node startNode = worldToNavGrid(request.start);
            Node targetNode = worldToNavGrid(request.end);

            Vector2[] navigation = {};
            bool successful = false;

            if (startNode.traversable && targetNode.traversable){

                Heap<Node> open = new Heap<Node>(nodesX * nodesY);
                HashSet<Node> closed = new HashSet<Node>();

                open.add(startNode);

                while (open.getSize() > 0){
                    Node currentNode = open.removeFirst();
                    closed.Add(currentNode);

                    if (currentNode == targetNode){
                        successful = true;
                        break;
                    }
                    foreach (Node neighbor in getNeighbors(currentNode)){
                        if (neighbor.traversable && !closed.Contains(neighbor)){
                            int newCost = currentNode.gCost + getDistance(currentNode, neighbor) + neighbor.movementPenalty;
                            if (newCost < neighbor.gCost || !open.contains(neighbor)){
                                neighbor.gCost = newCost;
                                neighbor.hCost = getDistance(neighbor, targetNode);
                                neighbor.pastNode = currentNode;
                                if (!open.contains(neighbor)) open.add(neighbor);
                                else open.updateItem(neighbor);
                            }
                        }
                    }
                }
            }
            if (successful){
                navigation = tracePath(startNode, targetNode);
                successful = navigation.Length > 0;
            }
            callback(new PathResult(navigation, successful, request.callback));
        }

        private Vector2[] tracePath(Node startNode, Node endNode){
            path = new List<Node>();
            Node currentNode = endNode;
            while (currentNode != startNode){
                path.Add(currentNode);
                currentNode = currentNode.pastNode;
            }
            Vector2[] navigation = simplifyPath(path);
            Array.Reverse(navigation);
            return navigation;
        }

        private Vector2[] simplifyPath(List<Node> path){
            List<Vector2> nav = new List<Vector2>();
            Vector2 oldDir = Vector2.zero;

            for (int i = 1; i < path.Count; i++){
                Vector2 newDir = new Vector2(path[i - 1].x - path[i].x, path[i - 1].y - path[i].y);
                if (newDir != oldDir){
                    nav.Add(path[i].position);
                }
                oldDir = newDir;
            }
            return nav.ToArray();
        }

        private int getDistance(Node node1, Node node2){
            int distanceX = Mathf.Abs(node1.x - node2.x);
            int distanceY = Mathf.Abs(node1.y - node2.y);

            if (distanceX > distanceY){
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }else{
                return 14 * distanceX + 10 * (distanceY - distanceX);
            }
        }

        void OnDrawGizmos(){
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));

            if (navGrid != null && collisionDebug){
                Node checkNode = null;
                foreach (Node node in navGrid){
                    if (onlyDisplayPath){
                        if (!path.Contains(node)) continue;
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(node.position, Vector3.one * (nodeSize));
                    }else{
                        if (node.traversable){
                            Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, node.movementPenalty));
                        }else{
                            Gizmos.color = Color.red;
                        }
                        if (checkNode == node) Gizmos.color = Color.blue;
                        if (path != null && path.Contains(node)) Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(node.position, Vector3.one * (nodeSize));
                    }
                }
            }
        }

        [System.Serializable]
        public class TerrainType{
            public LayerMask terrainMask;
            public int terrainPenalty;
        }
    }
}