using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    private Vector2 moveDirection;

    float horizontal;
    float vertical;

    bool canDash;

    private float saveRunSpeed;
    [SerializeField] float runSpeed = 20.0f;
    [SerializeField] float dashSpeed = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontal, vertical).normalized;
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(moveDirection.x * runSpeed, moveDirection.y * runSpeed);
    }

}
