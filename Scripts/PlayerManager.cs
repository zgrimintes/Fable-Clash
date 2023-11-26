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
    public float horizontalS;
    private float playerYScale;

    private float timeSinceTapped;
    private bool doubleTapped = false;
    private bool tapped = false;
    private float timeToDT = 0.4f;
    private int lastKey = 0; // -1 for A and 1 for D
    private bool isDashing = false;

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

        if (Input.GetKeyDown(KeyCode.J) && Time.time - lastAttack > cooldown)
        {
            try_NA();
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.time - lastAttack > cooldown)
        {
            try_RA();
        }
        if (Input.GetKeyDown(KeyCode.K) && Time.time - lastAttack > cooldown)
        {
            try_HA();
        }
        if (Input.GetKeyDown(KeyCode.I) && Time.time - lastAttack > cooldown)
        {
            try_MA();
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!tapped) tapped = true;
            else
            {
                if (Time.time - timeSinceTapped < timeToDT && lastKey == 1)
                {
                    doubleTapped = true;
                }

                tapped = false;
            }
            lastKey = 1;
            timeSinceTapped = Time.time;
        } //Dash for right

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!tapped) tapped = true;
            else
            {
                if (Time.time - timeSinceTapped < timeToDT && lastKey == -1)
                {
                    doubleTapped = true;
                }

                tapped = false;
            }
            timeSinceTapped = Time.time;
            lastKey = -1;
        } //Dash for left

        if (doubleTapped)
        {
            doubleTapped = false;
            StartCoroutine(Dashh());
        }
    }

    private IEnumerator Dashh()
    {
        isDashing = true;
        lastKey = 0;
        rb.velocity = new Vector2(horizontalS * speed * 3, rb.velocity.y);
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
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
