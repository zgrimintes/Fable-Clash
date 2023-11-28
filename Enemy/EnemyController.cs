using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyController : CharacterManager
{
    public GameObject playerInstance;
    public bool isTooFar = true;

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

        stateMachine.Initialize(chaseState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentEnemyState.FrameUpdate();

        if (transform.position.x > playerInstance.transform.position.x) transform.localScale = new Vector3(-1, transform.localScale.y);
        else transform.localScale = new Vector3(1, transform.localScale.y);
    }

    public void MoveEnemy(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}

