using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [HideInInspector] public AIAttacksBalaurul balaurulAI;
    [HideInInspector] public AIAttacksCrisnicul crisniculAI;
    #endregion

    float dirToKnock = -5;
    public GameObject playerInstance;
    public GameObject enemyInfoIcon;
    public bool isTooFar = true;
    public bool canAttack = false;
    public bool isAbove = false;
    [HideInInspector] public bool isJumping = false; //Remove if not use later
    [HideInInspector] public bool isSpecial = false;
    [HideInInspector] public float chance;

    float chanceToBlock = .5f;
    float lastBlock = 0;
    float lastWaited = 0;

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
        balaurulAI = GetComponent<AIAttacksBalaurul>();
        crisniculAI = GetComponent<AIAttacksCrisnicul>();
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
        if (enemyInfoIcon) enemyInfoIcon.GetComponent<Image>().sprite = icon;

        setAIScripts();
    }

    private void setAIScripts()
    {
        GetComponent<AIAttacksPrislea>().enabled = (prisleaAI.nameToHave == _ch_name);
        GetComponent<AIAttacksZmeu>().enabled = (zmeuAI.nameToHave == _ch_name);
        GetComponent<AIAttacksHarapAlb>().enabled = (harapalbAI.nameToHave == _ch_name);
        GetComponent<AIAttacksSpinul>().enabled = (spinulAI.nameToHave == _ch_name);
        GetComponent<AIAttacksGreuceanu>().enabled = (greuceanuAI.nameToHave == _ch_name);
        GetComponent<AIAttacksCapcaunul>().enabled = (capcaunuAI.nameToHave == _ch_name);
        GetComponent<AIAttacksZgripturoaica>().enabled = (zgripturoaciaAI.nameToHave == _ch_name);
        GetComponent<AIAttacksBalaurul>().enabled = (balaurulAI.nameToHave == _ch_name);
        GetComponent<AIAttacksCrisnicul>().enabled = (crisniculAI.nameToHave == _ch_name);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentEnemyState.FrameUpdate();

        if (HP < 7) chanceToBlock = 1f;
        if (HP < 4) chanceToBlock = 2f;

        if (transform.position.x - playerInstance.transform.position.x > 10f || transform.position.x - playerInstance.transform.position.x < -10f) //tryDash();
            if (playerInstance.GetComponent<CharacterManager>().isDashing) tryDashingOver();

        if (rb.velocity.y != 0) isJumping = true;
        else isJumping = false;

        if (transform.position.x > playerInstance.transform.position.x && transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        else if (transform.position.x < playerInstance.transform.position.x && transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);

        if (Random.Range(0, 4) < chanceToBlock) decideToBlock(1);

        if (Time.time - lastWaited > 6f + Random.Range(-1, 1))
        {
            waitToAttack();
            lastWaited = Time.time;
        }

        checkIfAbove();
        checkIfUnder();
    }

    public void decideToBlock(int i)
    {
        if (Time.time - playerInstance.GetComponent<CharacterManager>().lastAttack > playerInstance.GetComponent<CharacterManager>().cooldown)
            return;

        if (i == 2)
        {
            Block();
            return;
        }

        if (!canAttack) return;
        if (Time.time - lastBlock < 3f) return; //So it doesn't block to often

        lastBlock = Time.time;

        if (Physics2D.CircleCast(GetComponent<AttackManager>().attackPoint.position, 5f, Vector2.one, 0f, LayerMask.GetMask("Player")))
            Block();
    }

    public void MoveEnemy(Vector2 velocity)
    {
        //if (isJumping) return; //If jumping stop the wrinting of velocity
        if (isDashing) return; //If dashing stop the wrinting of velocity

        if (enemy.GetComponent<CharacterManager>().isDangerous)
        {
            if (chance > 1.2f)
            {
                velocity = new Vector2(-velocity.x, rb.velocity.y); dirToKnock = 5; Debug.Log("is dangerous");
            }
        }

        if (!isKnockback)
        {
            rb.velocity = new Vector2(velocity.x, rb.velocity.y);//Stop writing the velocity if you are getting knockbacked
            if (velocity != Vector2.zero) animator.SetFloat("speed", 1);
        }
        else
        {
            rb.velocity = new Vector2(velocity.x * dirToKnock, velocity.y);
            StartCoroutine(Knockback());
            dirToKnock = -2.5f;
        }
    }

    protected void checkIfAbove() //Check if the player is above so that can change states if so
    {
        if (Physics2D.BoxCast(transform.position + new Vector3(0, 3, 0), new Vector2(.2f, .2f), 0, Vector2.up, .1f, enemyLayer) && !isDashing)
            StartCoroutine(Dashh(.3f));
    }

    protected void checkIfUnder() //Check if the player is under so that can change states if so
    {
        if (Physics2D.BoxCast(transform.position + new Vector3(0, -3, 0), new Vector2(.2f, .2f), 0, Vector2.down, .1f, enemyLayer) && !isDashing)
            StartCoroutine(Dashh(.2f));
    }

    protected void tryDash()
    {
        if (playerInstance.GetComponent<CharacterManager>().isDangerous) return; //Don't go near the dangerous enemy
        if (isSpecial) return;
        if (isDashing || !canAttack) return; //Don't try if you are not allowed

        if ((int)Random.Range(0, 550) <= 2) //Added a chance to dash to you
        {
            StartCoroutine(Dashh(.2f));
        }
    }

    protected void tryDashingOver()
    {
        if (isDashing || !canAttack) return;

        int r = (int)Random.Range(0, 550);
        if (r <= 2)
        {
            Jump();
            StartCoroutine(Dashh(.2f));
        }
    }

    protected async void waitToAttack()
    {
        if (!canAttack) return;

        stopAllAnims();
        float chanceToWait = Random.Range(0, 2.5f);

        if (chanceToWait < 1)
        {
            stateMachine.Change(waitState);

            await Task.Delay(1400);

            if (hasLost || playerInstance.GetComponent<CharacterManager>().hasLost) return;
            stateMachine.Change(chaseState);
        }
        else if (chanceToWait < 2)
        {
            GetComponent<AIAttacksPrislea>().enabled =
            GetComponent<AIAttacksZmeu>().enabled =
            GetComponent<AIAttacksHarapAlb>().enabled =
            GetComponent<AIAttacksSpinul>().enabled =
            GetComponent<AIAttacksGreuceanu>().enabled =
            GetComponent<AIAttacksCapcaunul>().enabled =
            GetComponent<AIAttacksZgripturoaica>().enabled =
            GetComponent<AIAttacksBalaurul>().enabled =
            GetComponent<AIAttacksCrisnicul>().enabled = false;
            Debug.Log("Stopped attacking");

            await Task.Delay(2400); //Wait a second until can attack again

            Debug.Log("Back to attack");
            setAIScripts();
        }
    }
}
