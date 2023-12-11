using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : EnemyState
{
    protected float timeSinceStopped;
    public float attackColldown = 2f;
    private int attackNb = 0;
    private bool dashed = false;

    public WaitState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        timeSinceStopped = Time.time;
        dashed = false;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (Time.time - timeSinceStopped > attackColldown)
        {
            timeSinceStopped = Time.time;
            attackCycle();
            //enemy.GetComponent<CharacterManager>().try_RA();
        }

        if (enemy.isTooFar)
        {
            if (!dashed)
            {
                enemy.StartCoroutine(enemy.Dashh());
                enemy.try_NA();
                dashed = true;
            }
        }

        if (!enemy.isDashing && dashed) enemy.stateMachine.Change(enemy.chaseState);
    }

    protected void attackCycle()
    {
        switch (attackNb)
        {
            case 0:
                enemy.GetComponent<CharacterManager>().try_RA();
                attackNb = 1;
                break;
            case 1:
                enemy.GetComponent<CharacterManager>().try_MA();
                attackNb = 2;
                break;
            case 2:
                enemy.GetComponent<CharacterManager>().try_SA();
                attackNb = 0;
                break;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
