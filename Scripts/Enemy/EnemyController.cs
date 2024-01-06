using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : CharacterManager
{
    #region References to AI scritps
    [HideInInspector] public AIAttacksZmeu zmeuAI;
    [HideInInspector] public AIAttacksPrislea prisleaAI;
    [HideInInspector] public AIAttacksHarapAlb harapalbAI;
    [HideInInspector] public AIAttacksSpinul spinulAI;
    [HideInInspector] public AIAttacksGreuceanu greuceanuAI;
    [HideInInspector] public AIAttacksCapcaunul capcaunuAI;
    [HideInInspector] public AIAttacksZgripturoaica zgripturoaciaAI;
    #endregion

    float dirToKnock = -5;
    public GameObject playerInstance;
    public bool isTooFar = true;
    public bool canAttack = false;
    public bool isAbove = false;
    [HideInInspector] public bool isJumping = false; //Remove if not use later
    [HideInInspector] public bool isSpecial = false;

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

        //Get script references
        zmeuAI = GetComponent<AIAttacksZmeu>();
        prisleaAI = GetComponent<AIAttacksPrislea>();
        harapalbAI = GetComponent<AIAttacksHarapAlb>();
        spinulAI = GetComponent<AIAttacksSpinul>();
        greuceanuAI = GetComponent<AIAttacksGreuceanu>();
        capcaunuAI = GetComponent<AIAttacksCapcaunul>();
        zgripturoaciaAI = GetComponent<AIAttacksZgripturoaica>();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(waitState);
        choseAIScript();
    }

    public void choseAIScript()
    {
        GameObject iconE = GameObject.Find("CharacterIconEnemy");//Set the icon in UI
        iconE.GetComponent<Image>().sprite = icon;

        GetComponent<AIAttacksPrislea>().enabled = (prisleaAI.nameToHave == _ch_name);
        GetComponent<AIAttacksZmeu>().enabled = (zmeuAI.nameToHave == _ch_name);
        GetComponent<AIAttacksHarapAlb>().enabled = (harapalbAI.nameToHave == _ch_name);
        GetComponent<AIAttacksSpinul>().enabled = (spinulAI.nameToHave == _ch_name);
        GetComponent<AIAttacksGreuceanu>().enabled = (greuceanuAI.nameToHave == _ch_name);
        GetComponent<AIAttacksCapcaunul>().enabled = (capcaunuAI.nameToHave == _ch_name);
        GetComponent<AIAttacksZgripturoaica>().enabled = (zgripturoaciaAI.nameToHave == _ch_name);
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
        //if (isJumping) return; //If jumping stop the wrinting of velocity
        if (isDashing) return; //If dashing stop the wrinting of velocity

        if (enemy.GetComponent<CharacterManager>().isDangerous) { velocity = new Vector2(-velocity.x, velocity.y); dirToKnock = 5; Debug.Log("is dangerous"); }

        if (!isKnockback) rb.velocity = velocity;//Stop writing the velocity if you are getting knockbacked
        else
        {
            rb.velocity = new Vector2(velocity.x * dirToKnock, velocity.y);
            StartCoroutine(Knockback());
            dirToKnock = -5;
        }
    }

    protected void checkIfAbove() //Check if the player is above so that can change states if so
    {
        if (Physics2D.BoxCast(transform.position + new Vector3(0, 3, 0), new Vector2(.2f, .2f), 0, Vector2.up, .1f, enemyLayer) && !isDashing)
            StartCoroutine(Dashh());
    }

    protected void tryDash()
    {
        if (playerInstance.GetComponent<CharacterManager>().isDangerous) return; //Don't go near the dangerous enemy
        if (isSpecial) return;
        if (isDashing || !canAttack) return; //Don't try if you are not allowed

        if ((int)Random.Range(0, 550) <= 2) //Added a chance to dash to you
        {
            StartCoroutine(Dashh());
        }
    }

    protected void tryDashingOver()
    {
        if (isDashing || !canAttack) return;

        int r = (int)Random.Range(0, 550);
        if (r <= 2)
        {
            Jump();
            StartCoroutine(Dashh());
        }
    }
}
