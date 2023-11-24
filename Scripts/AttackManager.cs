using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private GameObject Weapon;
    [SerializeField] private GameObject Projectile;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float flying_speed = 0.2f;
    public LayerMask enemyLayer;
    GameObject wp;

    public void normal_Attack()
    {
        wp = Instantiate(Weapon, transform.position, Quaternion.identity);
        if (GetComponent<PlayerManager>().horizontalS == -1) wp.GetComponent<Animator>().Play("SwordSwingLeft");
        else if (GetComponent<PlayerManager>().horizontalS == 1) wp.GetComponent<Animator>().Play("SwordSwing");

        wp.AddComponent<FollowPlayer>().toFollow = gameObject; //Adding the FollowPlayer

        checkForColls(attackPoint.position, attackRange);
    }

    public void ranged_Attack()
    {
        float dir = GetComponent<PlayerManager>().horizontalS;

        if (dir >= 0) wp = Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, 270));
        else wp = Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, 90));

        StartCoroutine(ranged(dir));

    }

    public IEnumerator ranged(float dir)
    {
        while (Physics2D.OverlapBox(wp.transform.position, wp.transform.localScale, 0, enemyLayer) == null && !outOfBounds(wp))
        {
            wp.transform.position = wp.transform.position + new Vector3(flying_speed * dir, 0, 0);
            yield return null;
        }

        checkForColls(wp.transform.position, 1f);

        Destroy(wp);
    }

    public bool outOfBounds(GameObject gameObj)
    {
        if (gameObj.transform.position.x > 14f || gameObj.transform.position.x < -14f) return true;

        return false;
    }

    public void checkForColls(Vector2 point, float radius)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(point, radius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharacterManager>().take_damage(1);
        }
    }
}
