using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicAbilitiesManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject Mace;
    public GameObject Mist;
    public GameObject Powder;
    public GameObject Boomerang;
    AttackManager attackManager;
    CharacterManager characterManager;

    bool damageTaken = false;

    private void Start()
    {
        attackManager = GetComponent<AttackManager>();
        characterManager = GetComponent<CharacterManager>();
    }

    public void Praslea_MA(GameObject wp)
    {
        wp = Instantiate(Mace, transform.position, Quaternion.identity);

        attackManager.wp = wp;
    }

    public void Zmeul_MA()
    {
        Instantiate(Mist, new Vector3(transform.position.x + (characterManager.horizontalS * 5), -2f), Quaternion.identity);
    }

    public void HarapAlb_MA()
    {
        //Take more time for effects to go out
        characterManager.timeToGetRidOfEffects = 3f;

        //Double dmg for attacks
        characterManager.applyEfects(2);

        //Visual effects
        characterManager.popUpText("boost", 3);

        characterManager.lastAttack -= 1f; //Reset the lastAttack because it isn't an attack
    }

    public void Spinul_MA()
    {
        attackManager.normalNextAttack = false;
        attackManager.dmg += 1;
        Vector2 teleportTo = new Vector2(enemy.transform.position.x + -4f * enemy.GetComponent<CharacterManager>().horizontalS, enemy.transform.position.y);
        if (canTeleportTo(teleportTo))
        {
            transform.position = teleportTo;
        }
        else
        {
            Debug.Log("Failed to Teleport");
            GetComponent<CharacterManager>().fighterManager.mana += 2;
        }

        GetComponent<CharacterManager>().lastAttack -= 1f; //Reset the lastAttack because it isn't an attack
    }

    public void Greuceanul_MA()
    {
        enemy.GetComponent<CharacterManager>().applyEfects(3);//Make him sleep
    }

    public void Capcaunul_MA()
    {
        enemy.GetComponent<CharacterManager>().applyEfects(4);
    }

    public void Zgripturoaica_MA()
    {
        GameObject projectile = Instantiate(Powder, transform.position, Quaternion.identity);

        attackManager.wp = projectile;
    }

    public void Balaurul_MA()
    {
        GameObject boomerang = Instantiate(Boomerang, transform.position, Quaternion.identity);
        StartCoroutine(blazingBoomerang(boomerang, characterManager.horizontalS));
    }

    IEnumerator blazingBoomerang(GameObject obj, float dir)
    {
        int turns = 0;

        while (turns < 4)
        {
            if (obj == null) break;

            obj.transform.position += new Vector3(20f * dir * Time.deltaTime, 0);

            if (!damageTaken)
            {
                if (Physics2D.OverlapCircle(obj.transform.position, .4f, LayerMask.GetMask("Enemies")))
                {
                    damageTaken = true;
                    enemy.GetComponent<CharacterManager>().take_damage(2);
                }
            }

            if (obj.transform.position.x < -12f || obj.transform.position.x > 12f)
            {
                dir *= -1;
                turns++;
                damageTaken = false;
            }

            yield return null;
        }

        Destroy(obj);
    }

    public bool canTeleportTo(Vector2 teleportPos)
    {
        Vector2 teleportPosCheck = new Vector2(teleportPos.x, teleportPos.y + .5f);
        if (Physics2D.BoxCast(teleportPosCheck, GetComponent<CharacterManager>().coll.bounds.size, 0f, Vector2.up, 0f, LayerMask.GetMask("Default")))
            return false;
        return true;
    }
}