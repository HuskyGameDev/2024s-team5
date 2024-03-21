using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nc543.Nav2D{
    public class NavAgent : MonoBehaviour{
        public Transform target;
        float speed = 5;
        [SerializeField] float pathRefresh = .25f;
        Vector2[] path;
        int targetIndex;

        void Start(){
            StartCoroutine(updatePath());
        }

        public void onPathFound(Vector2[] newPath, bool pathFound){
            if (pathFound){
                path = newPath;
                StopCoroutine("followPath");
                targetIndex = 0;
                StartCoroutine("followPath");
            }
        }

        private IEnumerator updatePath(){
            if (Time.timeSinceLevelLoad < .3f){
                yield return new WaitForSeconds(.3f);
            }
            while (true){
                yield return new WaitForSeconds(pathRefresh);
                if (target != null) PathRequestManager.requestPath(new PathRequest(transform.position, target.position, onPathFound));
            }
        }

        private IEnumerator followPath(){
            Vector2 currentPoint = path[0];

            while (true){
                if (new Vector2(transform.position.x, transform.position.y) == currentPoint){
                    targetIndex++;
                    if (targetIndex >= path.Length){
                        yield break;
                    }
                    currentPoint = path[targetIndex];
                }

                transform.position = Vector2.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);
                yield return null;
            }
        }

        public void OnDrawGizmos(){
            if (path != null){
                for (int i = targetIndex; i < path.Length; i++){
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(path[i], new Vector3(0.2f, 0.2f, 0.2f));

                    if (i == targetIndex){
                        Gizmos.DrawLine(transform.position, path[i]);
                    }else{
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}