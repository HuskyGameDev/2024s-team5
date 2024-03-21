using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace nc543.Nav2D{
    public class PathRequestManager : MonoBehaviour{

        private NavigationGrid navGrid;
        public static PathRequestManager instance;
        Queue<PathResult> results = new Queue<PathResult>();

        void Awake(){
            if (instance == null){
                instance = this;
                navGrid = GetComponent<NavigationGrid>();
            }else{
                Destroy(this);
            }
        }

        void Update(){
            if (results.Count > 0){
                int items = results.Count;
                lock(results){
                    for (int i = 0; i < items; i++){
                        PathResult result = results.Dequeue();
                        result.callback(result.path, result.success);
                    }
                }
            }
        }

        public static void requestPath(PathRequest request){
            ThreadStart threadStart = delegate{
                instance.navGrid.findPath(request, instance.finishedPath);
            };
            threadStart.Invoke();
        }

        public void finishedPath(PathResult result){
            lock(results){
                results.Enqueue(result);
            }
        }
    }

    public struct PathRequest{
        public Vector2 start;
        public Vector2 end;
        public Action<Vector2[], bool> callback;

        public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback){
            start = _start;
            end = _end;
            callback = _callback;
        }
    }

    public struct PathResult{
        public Vector2[] path;
        public bool success;
        public Action<Vector2[], bool> callback;

        public PathResult(Vector2[] path, bool success, Action<Vector2[], bool> callback){
            this.path = path;
            this.success = success;
            this.callback = callback;
        }
    }
}