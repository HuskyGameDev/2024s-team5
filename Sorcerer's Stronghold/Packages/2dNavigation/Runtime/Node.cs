using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nc543.Nav2D{
    public class Node : IHeapItem<Node>{
        public bool traversable;
        public Vector2 position;
        public int x;
        public int y;
        public int gCost;
        public int hCost;
        public Node pastNode;
        public int movementPenalty;
        int heapIndex;

        public Node(bool _traversable, Vector2 _position, int _x, int _y, int _penalty){
            traversable = _traversable;
            position = _position;
            x = _x;
            y = _y;
            movementPenalty = _penalty;
        }

        public int fCost{
            get{
                return gCost + hCost;
            }
        }

        public int HeapIndex{
            get{
                return heapIndex;
            }set{
                heapIndex = value;
            }
        }

        public int CompareTo(Node comparing){
            int compare = fCost.CompareTo(comparing.fCost);
            if (compare == 0){
                compare = hCost.CompareTo(comparing.hCost);
            }
            return -compare;
        }
    }
}