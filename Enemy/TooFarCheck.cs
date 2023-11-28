using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooFarCheck : MonoBehaviour
{
    public GameObject playerTarget;
    private EnemyController _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<EnemyController>();

        playerTarget = _enemy.playerInstance.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerTarget)
        {
            _enemy.isTooFar = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == playerTarget)
        {
            _enemy.isTooFar = true;
        }
    }
}
