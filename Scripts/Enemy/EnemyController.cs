using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyController : CharacterManager
{
    public AIAttacksZmeu zmeuAI;
    public GameObject playerInstance;
    public bool isTooFar = true;
    public bool canAttack = false;

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
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(waitState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentEnemyState.FrameUpdate();

        if (transform.position.x > playerInstance.transform.position.x && transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        else if (transform.position.x < playerInstance.transform.position.x && transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
    }

    public void MoveEnemy(Vector2 velocity)
    {
        if (!isKnockback) rb.velocity = velocity;//Stop writing the velocity if you are getting knockbacked
        else
        {
            rb.velocity = new Vector2(velocity.x * -5, velocity.y);
            StartCoroutine(Knockback());
        }
    }
}
