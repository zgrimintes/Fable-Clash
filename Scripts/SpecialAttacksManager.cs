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
    int index;
    public float fallingSpeed;

    private void Start()
    {
        attackManager = GetComponent<AttackManager>();
    }

    public void Praslea_SA(GameObject wp, GameObject projectile)
    {
        hits_Praslea = 0;
        wp = Instantiate(projectile, transform.position, Quaternion.identity);
        StartCoroutine(fly(wp));

        for (index = 0; index < 7; index++)
        {
            rainStartPoints[index] = Random.Range(enemy.transform.position.x - 6f, enemy.transform.position.x + 6f); //Spaw in proximity to the enemy
            projectiles[index] = Instantiate(projectile, new Vector3(rainStartPoints[index], 15, 0), Quaternion.Euler(0, 0, 180));
        }

        for (index = 0; index < 7; index++)
        {
            StartCoroutine(fall(projectiles[index]));
        }
    }

    public void Zmeul_SA(float dir)
    {
        if (gameObject.name == "Enemy") GetComponent<EnemyController>().isSpecial = true;
        StartCoroutine(charge(dir));

    }

    public void HarapAlb_SA(GameObject character)
    {
        GetComponent<CharacterManager>().jumpForce = 25f;
        GetComponent<CharacterManager>().fallGravityScale = 17f;
        GetComponent<CharacterManager>().Jump();
        StartCoroutine(fallLunge(character));
    }

    public void Spinul_SA()
    {
        GetComponent<CharacterManager>().speed -= 6f;
        GetComponent<CharacterManager>().canDash = false;
        Debug.Log("Start Rotating");
        StartCoroutine(rotate());
    }

    public IEnumerator rotate()
    {
        float startTime = Time.time;
        while (Time.time - startTime < 5f && !enemy.GetComponent<CharacterManager>().hasLost)
        {
            if (Physics2D.CircleCast(transform.position, 2.5f, Vector2.zero, 0, layer))
            {
                GetComponent<CharacterManager>().isKnockback = true;
                StartCoroutine(GetComponent<CharacterManager>().Knockback());
                attackManager.checkForColls(transform.position, 2.5f, 0);
                yield return new WaitForSeconds(.5f);
            }
            yield return 0;
        }

        Debug.Log("Finish Rotating");
        GetComponent<CharacterManager>().speed += 6;
        GetComponent<CharacterManager>().canDash = true;
    }

    public IEnumerator fallLunge(GameObject character)
    {
        while (character.GetComponent<CharacterManager>().rb.velocity.y > 0)
        {
            yield return 0;
        }

        while (character.GetComponent<CharacterManager>().rb.velocity.y < 0)
        {
            yield return 0;
        }

        character.GetComponent<CharacterManager>().jumpForce = 21f;
        character.GetComponent<CharacterManager>().fallGravityScale = 5f;

        attackManager.dmg = 4;
        attackManager.checkForColls(transform.position, 5f, 0);
    }

    public IEnumerator charge(float dir)
    {
        float speed = 0.001f;
        while (!attackManager.outOfBounds(gameObject) && Physics2D.OverlapBox(gameObject.transform.position, gameObject.GetComponent<BoxCollider2D>().size * .3f, 0, layer) == null)
        {
            gameObject.transform.position += new Vector3(speed * dir, 0, 0);
            speed += Time.deltaTime / 8;
            yield return null;
        }

        if (gameObject.name == "Enemy") GetComponent<EnemyController>().isSpecial = false;
        attackManager.checkForColls(transform.position, 3.3f, 0);
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
        while (wp.transform.position.y > -2f)
        {
            if (Physics2D.OverlapBox(wp.transform.position, new Vector2(0.3f, 0.3f), 0, layer) != null)
            {
                hits_Praslea++;
                break;
            }

            wp.transform.position = wp.transform.position - new Vector3(0, fallingSpeed * Time.deltaTime, 0);
            yield return null;
        }

        GetComponent<CharacterManager>().enemy.GetComponent<CharacterManager>().take_damage(hits_Praslea); hits_Praslea = 0;
        Destroy(wp);
    }
}
