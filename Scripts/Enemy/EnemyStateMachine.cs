using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState;

    public void Initialize(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;
        CurrentEnemyState.EnterState();
    }

    public void Change(EnemyState enemyState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = enemyState;
        CurrentEnemyState.EnterState();
    }
}

