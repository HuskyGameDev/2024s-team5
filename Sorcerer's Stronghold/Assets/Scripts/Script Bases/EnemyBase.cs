using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stronghold.Base;

namespace Stronghold.Base{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBase : EntityBase{
        [SerializeField] private float passiveTimer = 5f;
        [SerializeField] private EntityBase preferredTarget;
        [SerializeField] private Elements preferredElement;
        [SerializeField] private bool prefersResourceTowers;
        private float timer = 0f;

        private void movementAI(){}
        private void passiveAbility(){}

        void FixedUpdate(){
            timer += Time.fixedDeltaTime;
            movementAI();
            if (timer >= passiveTimer){
                timer -= passiveTimer;
                passiveAbility();
            }
        }

        void getTarget(){

        }
    }
}
