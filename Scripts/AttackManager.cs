using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private GameObject Weapon;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    GameObject wp;

    public void normal_Attack()
    {
        wp = Instantiate(Weapon, transform.position, Quaternion.identity);
        if (GetComponent<PlayerManager>().horizontalS == -1) wp.GetComponent<Animator>().Play("SwordSwingLeft");
        else if (GetComponent<PlayerManager>().horizontalS == 1) wp.GetComponent<Animator>().Play("SwordSwing");

        wp.AddComponent<FollowPlayer>().toFollow = gameObject; //Adding the FollowPlayer

        checkForColls();
    }

    public void checkForColls()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().take_damage(1);
        }
    }
}
