using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [SerializeField] private LayerMask layer;

    private bool isGrounded; //To check if the player touches the ground
    private float horizontal;
    private float playerYScale;

    protected override void Start()
    {
        base.Start();

        playerYScale = GetComponent<Transform>().localScale.y;
    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(horizontal * speed, rb.velocity.y);

        if (!isDashing) rb.velocity = movement;
    }

    protected override void Update()
    {
        base.Update();

        Dash();
        if (!isDashing && Input.GetKeyDown(KeyCode.Space)) Jump();

        if (horizontal == -1) { gameObject.transform.localScale = new Vector3(-1, playerYScale, 1); horizontalS = -1; }
        else if (horizontal == 1) { gameObject.transform.localScale = new Vector3(1, playerYScale, 1); horizontalS = 1; }

        if (Input.GetKeyDown(KeyCode.J))
        {
            try_NA();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            try_RA();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            try_HA();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            try_MA();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            try_SA();
        }
    }

    private void Jump()
    {
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, layer);

        if (isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
        }

        if (rb.velocity.y > 0) rb.gravityScale = gravityScale;
        else rb.gravityScale = fallGravityScale;
    }
}
