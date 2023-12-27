using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyController : CharacterManager
{
    #region References to AI scritps
    public AIAttacksZmeu zmeuAI;
    public AIAttacksPrislea prisleaAI;
    #endregion

    public GameObject playerInstance;
    public bool isTooFar = true;
    public bool canAttack = false;
    public bool isAbove = false;
    private bool isJumping = false;

    #region State Machine Variables

    public EnemyStateMachine stateMachine;
    public ChaseState chaseState;
    public WaitState waitState;

    #endregion

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        chaseState = new ChaseState(this, stateMachine);
        waitState = new WaitState(this, stateMachine);
        zmeuAI = GetComponent<AIAttacksZmeu>();
        prisleaAI = GetComponent<AIAttacksPrislea>();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(waitState);
        choseAIScript();
    }

    protected void choseAIScript()
    {
        GetComponent<AIAttacksPrislea>().enabled = (prisleaAI.nameToHave == _ch_name);
        GetComponent<AIAttacksZmeu>().enabled = (zmeuAI.nameToHave == _ch_name);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentEnemyState.FrameUpdate();

        if (transform.position.x - playerInstance.transform.position.x > 10f || transform.position.x - playerInstance.transform.position.x < -10f) tryDash();
        if (playerInstance.GetComponent<CharacterManager>().isDashing) tryDashingOver();

        if (rb.velocity.y != 0) isJumping = true;
        else isJumping = false;

        if (transform.position.x > playerInstance.transform.position.x && transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        else if (transform.position.x < playerInstance.transform.position.x && transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);

        checkIfAbove();
    }

    public void MoveEnemy(Vector2 velocity)
    {
        if (isJumping) return; //If jumping stop the wrinting of velocity
        if (isDashing) return; //If dashing stop the wrinting of velocity

        if (!isKnockback) rb.velocity = velocity;//Stop writing the velocity if you are getting knockbacked
        else
        {
            rb.velocity = new Vector2(velocity.x * -5, velocity.y);
            StartCoroutine(Knockback());
        }
    }

    protected void checkIfAbove() //Check if the player is above so that can change states if so
    {
        if (Physics2D.BoxCast(transform.position + new Vector3(0, 3, 0), new Vector2(.2f, .2f), 0, Vector2.up, .1f, enemyLayer) && !isDashing)
            StartCoroutine(Dashh());
    }

    protected void tryDash()
    {
        if (isDashing || !canAttack) return; //Don't try if you are not allowed

        if ((int)Random.Range(0, 1000) <= 2) //Added a chance to dash to you
        {
            StartCoroutine(Dashh());
        }
    }

    protected void tryDashingOver()
    {
        if (isDashing || !canAttack) return;

        int r = (int)Random.Range(0, 1000);
        if (r <= 1)
        {
            Jump();
            StartCoroutine(Dashh());
        }
    }
}
