using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stronghold.Base{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]

    public class TowerBase : EntityBase{
        [SerializeField] protected float range = 10f;
        [SerializeField] protected Collider2D sightBox; 
        [SerializeField] protected Transform fireLoc;
        public float attackSpeed = 1f;
        public int targetMode = 0;
        public bool resourceTower = false;

        protected List<EnemyBase> targetableEnemies = new List<EnemyBase>();
        protected EnemyBase target;

        void Start(){
            Data.database.towers.Add(this);
            if (sightBox.GetType() == typeof(CircleCollider2D)){
                ((CircleCollider2D) sightBox).radius = range;
            }else if (sightBox.GetType() == typeof(BoxCollider2D)){
                ((BoxCollider2D) sightBox).size = new Vector2(range, range);
            }
        }

        void OnTriggerEnter2D(Collider2D collided){
            if (collided.tag == "Enemy"){
                targetableEnemies.Add(collided.GetComponent<EnemyBase>());
            }
        }

        void OnTriggerExit2D(Collider2D exited){
            if (exited.tag == "Enemy"){
                targetableEnemies.Remove(exited.GetComponent<EnemyBase>());
            }
        }

        /*
        This method gets an enemy based on the mode that is specified.
        The first mode (0) will be the closest enemy to the tower. 
        The second mode (1) will the the furthest reachable enemy to the tower. 
        The third mode (2) will be the enemy with the lowest amount of health. 
        The third mode (3) will be the enemy with the most amount of health. 
        */
        protected EnemyBase getFavoredEnemy(){
            switch (targetMode){
                case 0:{
                    // Return closest
                    float distance = float.PositiveInfinity;
                    int targetIndex = -1;
                    for (int i = 0; i < targetableEnemies.Count; i++){
                        float dist = Vector2.Distance(transform.position, targetableEnemies[i].gameObject.transform.position);
                        if (dist < distance && targetableEnemies[i].isAlive()){
                            distance = dist;
                            targetIndex = i;
                        }
                    }
                    return targetIndex != -1 ? targetableEnemies[targetIndex] : null;
                }case 1:{
                    // Return furthest
                    float distance = float.NegativeInfinity;
                    int targetIndex = -1;
                    for (int i = 0; i < targetableEnemies.Count; i++){
                        float dist = Vector2.Distance(transform.position, targetableEnemies[i].gameObject.transform.position);
                        if (dist > distance){
                            distance = dist;
                            targetIndex = i;
                        }
                    }
                    return targetIndex != -1 ? targetableEnemies[targetIndex] : null;
                }case 2:{
                    // Return lowest health
                    float health = float.PositiveInfinity;
                    int targetIndex = -1;
                    for (int i = 0; i < targetableEnemies.Count; i++){
                        if (targetableEnemies[i].getHealth() < health){
                            targetIndex = i;
                            health = targetableEnemies[i].getHealth();
                        }
                    }
                    return targetIndex != -1 ? targetableEnemies[targetIndex] : null;
                }case 3:{
                    // Return highest health
                    float health = float.NegativeInfinity;
                    int targetIndex = -1;
                    for (int i = 0; i < targetableEnemies.Count; i++){
                        if (targetableEnemies[i].getHealth() > health){
                            targetIndex = i;
                            health = targetableEnemies[i].getHealth();
                        }
                    }
                    return targetIndex != -1 ? targetableEnemies[targetIndex] : null;
                }default:{
                    return null;
                }
            }
        }

        protected override void onDeath(){
            alive = false;
            Data.database.towers.Remove(this);
            Debug.Log("Tower Died");
            die();
        }
    }
}
