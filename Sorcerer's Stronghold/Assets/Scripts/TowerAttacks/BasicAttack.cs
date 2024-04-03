using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stronghold.Base;

[RequireComponent(typeof(Collider2D))]
public class BasicAttack : MonoBehaviour{

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject deathEffect;
    private EntityBase.Elements element = EntityBase.Elements.Normal;
    private GameObject target;

    // Update is called once per frame
    void FixedUpdate(){
        if (target != null){
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.fixedDeltaTime);
        }else{
            die();
        }
    }

    public void setTarget(GameObject targ){
        target = targ;
    }

    public void setElement(EntityBase.Elements e){
        element = e;
    }

    void OnTriggerEnter2D(Collider2D collided){
        if (collided.gameObject == target){
            target.GetComponent<EntityBase>().alterHealth(-damage, element);
            die();
        }
    }

    void die(){
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
