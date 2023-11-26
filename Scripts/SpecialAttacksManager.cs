using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttacksManager : MonoBehaviour
{
    AttackManager attackManager;
    public GameObject enemy;
    public LayerMask layer;
    GameObject[] projectiles = new GameObject[7];
    float[] rainStartPoints = new float[7];

    int hits_Praslea = 0;

    private void Start()
    {
        attackManager = GetComponent<AttackManager>();
    }

    public int Praslea_SA(GameObject wp, GameObject projectile)
    {
        wp = Instantiate(projectile, transform.position, Quaternion.identity);
        StartCoroutine(fly(wp));

        for (int index = 0; index < 7; index++)
        {
            rainStartPoints[index] = Random.Range(enemy.transform.position.x - 7f, enemy.transform.position.x + 7f); //Spaw in proximity of the enemy
            projectiles[index] = Instantiate(projectile, new Vector3(rainStartPoints[index], 15, 0), Quaternion.Euler(0, 0, 180));
        }

        for (int index = 0; index < 7; index++)
        {
            StartCoroutine(fall(projectiles[index]));
        }

        return hits_Praslea;
    }

    public IEnumerator fly(GameObject wp)
    {
        while (wp.transform.position.y < 15f)
        {
            wp.transform.position = wp.transform.position + new Vector3(0, 0.2f, 0);
            yield return null;
        }

        Destroy(wp);
    }

    public IEnumerator fall(GameObject wp)
    {
        while (wp.transform.position.y > -2f && Physics2D.OverlapBox(wp.transform.position, new Vector2(0.3f, 0.3f), 0, layer) == null)
        {
            wp.transform.position = wp.transform.position - new Vector3(0, 0.05f, 0);
            yield return null;
        }

        if (Physics2D.OverlapBox(wp.transform.position, new Vector2(0.3f, 0.3f), 0, layer) != null) hits_Praslea++;

        Destroy(wp);
    }
}
