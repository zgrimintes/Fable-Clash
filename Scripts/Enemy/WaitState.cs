using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : EnemyState
{
    public WaitState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        enemy.canAttack = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.canAttack = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.MoveEnemy(Vector2.zero);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void StartFight()
    {
        stateMachine.Change(enemy.chaseState);
    }
}
