using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stronghold.Base;
using nc543.Nav2D;

namespace Stronghold.Base{
    //This ensures that every enemy has a rigidbody2D
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(NavAgent))]
    public class EnemyBase : EntityBase{
        [SerializeField] protected float passiveTimer = 5f;
        [SerializeField] protected Elements preferredElement;
        [SerializeField] protected bool prefersResourceTowers;
        //This determines how far out of a 
        [SerializeField] protected float preferredDetermination = 2f;
        protected EntityBase currentTarget;
        private float timer = 0f;
        protected NavAgent navigation;

        protected void movementAI(){}
        protected void passiveAbility(){}

        void Awake(){
            navigation = GetComponent<NavAgent>();
        }

        void FixedUpdate(){
            timer += Time.fixedDeltaTime;
            movementAI();
            if (timer >= passiveTimer){
                timer -= passiveTimer;
                passiveAbility();
            }
        }

        protected void getTarget(){
            float minTargetValue = 9999999;
            EntityBase targ = null;
            foreach (EntityBase target in Data.database.towers){
                float targetValue = Vector2.Distance(transform.position, target.transform.position);
                if (target is TowerBase && ((TowerBase) target).resourceTower && prefersResourceTowers) targetValue /= preferredDetermination;
                if (target.element == preferredElement) targetValue /= preferredDetermination;
                if (targetValue < minTargetValue){
                    minTargetValue = targetValue;
                    targ = target;
                }
            }
            foreach (EntityBase target in Data.database.player){
                float targetValue = Vector2.Distance(transform.position, target.transform.position);
                if (target is TowerBase && ((TowerBase) target).resourceTower && prefersResourceTowers) targetValue /= preferredDetermination;
                if (target.element == preferredElement) targetValue /= preferredDetermination;
                if (targetValue < minTargetValue){
                    minTargetValue = targetValue;
                    targ = target;
                }
            }
            currentTarget = targ;
            navigation.target = targ.gameObject.transform;
        }
    }
}
