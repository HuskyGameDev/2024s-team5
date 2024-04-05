using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stronghold.Base;

public class Imp : EnemyBase{
    private float attackTimer = float.PositiveInfinity;

    // Start is called before the first frame update
    void Start(){
        StartCoroutine("targeting");
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (currentTarget == null) {
            Debug.Log("Target was null");
            getTarget();
        };
        if (Vector2.Distance(transform.position, currentTarget.transform.position) < attackRange){
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer >= attackSpeed){
                currentTarget.alterHealth(-damage, element);
                attackTimer = 0f;
            }
        }
    }

    private IEnumerator targeting(){
        yield return new WaitForSeconds(0.5f);
        getTarget();
    }
}
