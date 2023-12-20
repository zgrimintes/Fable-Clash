using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : EnemyState
{
    private GameObject playerEnemy;

    public ChaseState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        playerEnemy = GameObject.FindGameObjectWithTag("Player");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!enemy.canAttack)
        {
            enemy.MoveEnemy(Vector2.zero);
            stateMachine.Change(enemy.waitState);
        }

        enemy.MoveEnemy(new Vector2(enemy.playerInstance.transform.position.x - enemy.transform.position.x, 0).normalized * (enemy.speed / 2));
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
