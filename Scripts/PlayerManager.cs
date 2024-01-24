using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : CharacterManager
{
    private float horizontal;
    private float playerYScale;
    private float playerXScale;
    public bool canMove;
    public GameObject playerInfoIcon;

    protected override void Start()
    {
        base.Start();

        playerYScale = GetComponent<Transform>().localScale.y;
        playerXScale = GetComponent<Transform>().localScale.x;
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(horizontal));

        Vector2 movement = new Vector2(horizontal * speed, rb.velocity.y);

        if (!isDashing && !isKnockback) rb.velocity = movement; //Stop the writing for the velocity if you dash or getting knockbacked
        if (isKnockback) //For when getting knockbacked
        {
            int kbHor = getKnHor();
            Vector2 movement2 = new Vector2(kbHor * speed, rb.velocity.y);
            rb.velocity = new Vector2(movement2.x * 1.1f, movement2.y);
            StartCoroutine(Knockback());
        }
    }

    public void changeIcon()
    {
        GameObject iconP = GameObject.Find("CharacterIconPlayer");//Set the icon in UI
        iconP.GetComponent<Image>().sprite = icon;
        if (playerInfoIcon) playerInfoIcon.GetComponent<Image>().sprite = icon;
    }

    private int getKnHor()
    {
        switch (attackDir)
        {
            case true:
                return 1;
            case false:
                return -1;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (!canMove) return;

        Dash();
        if (!isDashing && Input.GetKeyDown(KeyCode.Space)) Jump();

        if (horizontal == -1)
        {
            gameObject.transform.localScale = new Vector3(-playerXScale, playerYScale, 1);
            if (!isKnockback) horizontalS = -1;
        }
        else if (horizontal == 1)
        {
            gameObject.transform.localScale = new Vector3(playerXScale, playerYScale, 1);
            if (!isKnockback) horizontalS = 1;
        }

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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Block();
        }
    }
}
