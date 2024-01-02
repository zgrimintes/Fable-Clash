using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicAbilitiesManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject Mace;
    public GameObject Mist;
    AttackManager attackManager;

    private void Start()
    {
        attackManager = GetComponent<AttackManager>();
    }

    public void Praslea_MA(GameObject wp)
    {
        wp = Instantiate(Mace, transform.position, Quaternion.identity);

        attackManager.wp = wp;
    }

    public void Zmeul_MA()
    {
        Instantiate(Mist, new Vector3(transform.position.x + (GetComponent<CharacterManager>().horizontalS * 5), -2f), Quaternion.identity);
    }

    public void HarapAlb_MA()
    {
        //Take more time for effects to go out
        GetComponent<CharacterManager>().timeToGetRidOfEffects = 3f;

        //Double dmg attacks
        gameObject.GetComponent<CharacterManager>().applyEfects(2);

        GetComponent<CharacterManager>().lastAttack -= 1f; //Reset the lastAttack because it isn't an attack
    }

    public void Spinul_MA()
    {
        attackManager.normalNextAttack = false;
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

    public bool canTeleportTo(Vector2 teleportPos)
    {
        Vector2 teleportPosCheck = new Vector2(teleportPos.x, teleportPos.y + .5f);
        if (Physics2D.BoxCast(teleportPosCheck, GetComponent<CharacterManager>().coll.bounds.size, 0f, Vector2.up, 0f, LayerMask.GetMask("Default")))
            return false;
        return true;
    }
}