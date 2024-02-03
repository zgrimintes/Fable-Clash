using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttacksManager : MonoBehaviour
{
    AttackManager attackManager;
    CharacterManager characterManager;
    public GameObject enemy;
    public LayerMask layer;
    GameObject[] projectiles = new GameObject[7];
    float[] rainStartPoints = new float[7];
    float timeSinceLastHit = 0f;

    int hits_Praslea = 0;
    int index;
    public float fallingSpeed;

    public GameObject windShield;

    private void Start()
    {
        attackManager = GetComponent<AttackManager>();
        characterManager = GetComponent<CharacterManager>();
    }

    public void Praslea_SA(GameObject wp, GameObject projectile)
    {
        hits_Praslea = 0;
        wp = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
        StartCoroutine(fly(wp));

        for (index = 0; index < 7; index++)
        {
            rainStartPoints[index] = Random.Range(enemy.transform.position.x - 6f, enemy.transform.position.x + 6f); //Spaw in proximity to the enemy
            projectiles[index] = Instantiate(projectile, new Vector3(rainStartPoints[index], 15, 0), Quaternion.Euler(0, 0, -90));
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
        StartCoroutine(Lunge(character));
    }

    public void Spinul_SA()
    {
        characterManager.speed -= 6f;
        characterManager.canDash = false;
        characterManager.isDangerous = true;
        Debug.Log("Start Rotating");
        StartCoroutine(rotate());
    }

    public void Greuceanul_SA()
    {
        int hpGained = 5;
        characterManager.fighterManager.HP += hpGained;
        characterManager.HP += hpGained;

        if (characterManager.HP > 15)
        {
            hpGained = 5 - (characterManager.HP - 15);
            characterManager.fighterManager.HP = 15;
            characterManager.HP = 15;
        }

        characterManager.popUpText("heal", hpGained);
        GetComponent<SpriteRenderer>().color = new Color(0.5279903f, 1f, 0.5279903f, 1f);
    }

    public void Capcaunul_SA()
    {
        characterManager.canDash = false;
        characterManager.speed -= 6;
        StartCoroutine(hitTheGround());
    }

    public void Zgripturoaica_SA()
    {
        characterManager.applyEfects(6);
        GameObject wS = Instantiate(windShield, transform.position, Quaternion.identity);
        wS.AddComponent<FollowPlayer>();
        wS.GetComponent<FollowPlayer>().toFollow = gameObject;
    }

    public void Balaurul_SA()
    {
        characterManager.speed -= 8f;
        characterManager.canDash = false;
        characterManager.isDangerous = true;
        Debug.Log("Start Blasting");
        StartCoroutine(fireSpit());
    }

    public void Crisnicul_SA()
    {
        characterManager.applyEfects(7);
        characterManager.popUpText("boost", 4);
    }

    public IEnumerator fireSpit()
    {

        float startTime = Time.time;
        while (Time.time - startTime < 4.5f && !enemy.GetComponent<CharacterManager>().hasLost)
        {
            if (Physics2D.CircleCast(GetComponent<AttackManager>().attackPoint.transform.position, 2f, Vector2.zero, 0, layer))
            {
                attackManager.dmg = 2;
                attackManager.checkForColls(GetComponent<AttackManager>().attackPoint.transform.position, 2, 0);
                yield return new WaitForSeconds(.5f);
            }
            yield return null;
        }

        Debug.Log("Finish Blasting");
        characterManager.speed += 8;
        characterManager.canDash = true;
        characterManager.isDangerous = false;
    }

    public IEnumerator hitTheGround()
    {
        int hitsCapcaunul = 0;
        while (hitsCapcaunul < 4)
        {
            if (Time.time - timeSinceLastHit > .5f)
            {
                timeSinceLastHit = Time.time;
                hitsCapcaunul++;
                enemy.GetComponent<CharacterManager>().groundShake();
                CameraShake.Shake(.5f, .3f);
            }
            yield return null;
        }

        characterManager.canDash = true;
        characterManager.speed += 6;
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
        characterManager.speed += 6;
        characterManager.canDash = true;
        characterManager.isDangerous = false;
    }

    public IEnumerator Lunge(GameObject character)
    {
        float dir = characterManager.horizontalS;
        float speed = 20f;
        float lungeDir = 0;

        while (!attackManager.outOfBounds(gameObject) && Physics2D.OverlapBox(gameObject.transform.position, gameObject.GetComponent<BoxCollider2D>().size * .3f, 0, layer) == null && lungeDir < 20)
        {
            gameObject.transform.position += new Vector3(speed * dir * Time.deltaTime, 0, 0);
            lungeDir += .1f;
            yield return null;
        }

        attackManager.dmg = 4;
        attackManager.checkForColls(transform.position, 4f, 0);
        CameraShake.Shake(.3f, .3f);
    }

    public IEnumerator charge(float dir)
    {
        float speed = 10f;
        while (!attackManager.outOfBounds(gameObject) && Physics2D.OverlapBox(gameObject.transform.position, gameObject.GetComponent<BoxCollider2D>().size * .3f, 0, layer) == null)
        {
            gameObject.transform.position += new Vector3(speed * dir * Time.deltaTime, 0, 0);
            speed += Time.deltaTime * 50;
            yield return null;
        }

        GetComponent<SignalFinishAttack>().signalFinishAttack();
        if (gameObject.name == "Enemy") GetComponent<EnemyController>().isSpecial = false;
        attackManager.checkForColls(transform.position, 3.3f, 0);
        CameraShake.Shake(.2f, .2f);

        StopAllCoroutines();
    }

    public IEnumerator fly(GameObject wp)
    {
        while (wp.transform.position.y < 15f)
        {
            wp.transform.position = wp.transform.position + new Vector3(0, 80f * Time.deltaTime, 0);
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
        characterManager.animator.SetBool("isSpecial", false);
    }
}
