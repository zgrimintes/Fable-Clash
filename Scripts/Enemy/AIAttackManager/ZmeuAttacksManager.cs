using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttacksZmeu : MonoBehaviour
{
    public EnemyController enemyController;

    public void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public void Update()
    {
        if (!enemyController.isTooFar)
        {
            gameObject.GetComponent<CharacterManager>().try_NA();
        }
    }
}
