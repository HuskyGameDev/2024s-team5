using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stronghold.Base;

public class TestingTower: TowerBase{

    [SerializeField] private GameObject projectile;
    float timer = 0f;

    void Update(){
        timer += Time.deltaTime;
        if (timer >= attackSpeed){
            timer -= attackSpeed;
            attack();
        }
    }

    void attack(){
        EnemyBase target = getFavoredEnemy();
        if (target != null){
            BasicAttack shot = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<BasicAttack>();
            shot.setTarget(target.gameObject);
        }
    }
}