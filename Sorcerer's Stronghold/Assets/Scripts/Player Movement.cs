using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Stronghold.Base;

public class PlayerMovement : EntityBase
{
    [Header("Player Variables")]
    public Rigidbody2D body;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Animator animator;
    private Vector2 moveDirection;
   
    float horizontal;
    float vertical;

    private float saveRunSpeed;
    [SerializeField] float dashSpeed = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontal, vertical).normalized;

        if (moveDirection.x < 0){
            render.flipX = true;
            animator.SetBool("moving", true);
        }else if (moveDirection.x > 0){
            render.flipX = false;
            animator.SetBool("moving", true);
        }else{
            animator.SetBool("moving", false);
        }

        body.velocity = new Vector2(moveDirection.x * this.speed, moveDirection.y * speed);
    }

    protected override void onDeath(){

    }
}
