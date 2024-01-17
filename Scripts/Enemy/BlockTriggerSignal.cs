using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTriggerSignal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            GetComponentInParent<EnemyController>().decideToBlock(2);
        }
    }
}
