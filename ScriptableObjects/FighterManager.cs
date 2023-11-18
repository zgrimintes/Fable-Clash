using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FighterManager", menuName = "Figher Manager")]
public class FighterManager : ScriptableObject
{
    public int HP;
    public int mana;
    public ScriptableObject w_Class;
    public string characterName;

    public void normal_Attack(GameObject parent)
    {
        mana -= 1;
        parent.GetComponent<PlayerManager>().mana = mana;
    }
}
