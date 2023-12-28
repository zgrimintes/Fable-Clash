using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicAbilitiesManager : MonoBehaviour
{
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

        //Double attacks
        gameObject.GetComponent<CharacterManager>().applyEfects(2);
        gameObject.GetComponent<CharacterManager>().applyEfects(3);
        gameObject.GetComponent<CharacterManager>().applyEfects(4);
    }
}
