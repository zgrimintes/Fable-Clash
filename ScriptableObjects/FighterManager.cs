using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FighterManager", menuName = "Figher Manager")]
public class FighterManager : ScriptableObject
{
    public int HP;
    public int mana;
    public int stamina;
    public ScriptableObject w_Class;
    public string characterName;
    public int roundsWon;
    public Sprite sprite;
    public Sprite characterIcon;
    public float timeToGetRidOfEffects;
    public bool isBoss;

    public string[] Dialogues = new string[2]; //0 - WinDialogue; 1 - LoseDialogue;

    public void ranged_Attack(GameObject parent)
    {
        stamina -= 1;
        parent.GetComponent<CharacterManager>().stamina = stamina;
    }

    public void heavy_Attack(GameObject parent)
    {
        stamina -= 2;
        parent.GetComponent<CharacterManager>().stamina = stamina;
    }

    public void magic_Attack(GameObject parent)
    {
        mana -= 2;
        parent.GetComponent<CharacterManager>().mana = mana;
    }
    public void special_Attack(GameObject parent)
    {
        mana -= 3;
        parent.GetComponent<CharacterManager>().mana = mana;
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

    public void startOfFight()
    {
        if (isBoss) HP = 60;
        else HP = 20;

        mana = 4;
        stamina = 5;
    }
}
