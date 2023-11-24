using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FighterManager", menuName = "Figher Manager")]
public class FighterManager : ScriptableObject
{
    public int HP;
    public int mana;
    public int stamina;
    public ScriptableObject w_Class;
    public string characterName;
    public bool enemy;
    public int roundsWon;

    public void ranged_Attack(GameObject parent)
    {
        stamina -= 1;
        parent.GetComponent<CharacterManager>().stamina = stamina;
    }

    public void mana_Gain(GameObject parent)
    {
        mana += 1;
        parent.GetComponent<CharacterManager>().mana = mana;
    }

    public void stamina_Gain(GameObject parent)
    {
        stamina += 1;
        parent.GetComponent<CharacterManager>().stamina = stamina;
    }

    public void take_damage(GameObject parent, int damage)
    {
        HP -= damage;
        parent.GetComponent<CharacterManager>().HP = HP;
    }
}
