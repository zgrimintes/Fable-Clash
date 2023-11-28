using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : EnemyState
{
    protected float timeSinceStopped;

    public WaitState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        timeSinceStopped = Time.time;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (Time.time - timeSinceStopped > 2f)
        {
            timeSinceStopped = Time.time;
            enemy.GetComponent<CharacterManager>().try_RA();

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

