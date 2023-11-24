using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManager : AttackManager
{
    [HideInInspector] public float jumpForce = 15.5f;
    [HideInInspector] public float gravityScale = 4;
    [HideInInspector] public float fallGravityScale = 5;
    [HideInInspector] public int mana;
    [HideInInspector] public int HP;
    [HideInInspector] public int stamina;
    [HideInInspector] public float speed;
    [HideInInspector] public float cooldown = 0.8f;
    [HideInInspector] public float lastAttack;
    public FighterManager fighterManager;
    public GameObject textPrfb;

    public void LoadPlayer(FighterManager data) //Function for loading the data from the ScriptableObject into the GameObject
    {
        WeightClass w_data = (WeightClass)data.w_Class;
        speed = w_data.speed;
        mana = data.mana;
        HP = data.HP;
        stamina = data.stamina;
    }

    public void updateText()
    {
        string hName = "HealthP", mName = "ManaP ", sName = "StaminaP";
        if (fighterManager.enemy)
        {
            hName = "HealthE";
            mName = "ManaE";
            sName = "StaminaE";
        }

        TextMeshProUGUI[] children = textPrfb.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI child in children)
        {
            if (child == null) return;

            if (child.name == hName)
                child.text = "Health: " + HP;
            else if (child.name == mName)
                child.text = "Mana: " + mana;
            else if (child.name == sName)
                child.text = "Stamina: " + stamina;
        }
    }

    public void take_damage(int damage)
    {
        fighterManager.take_damage(gameObject, damage);
        updateText();
    }

    public void try_NA()
    {
        lastAttack = Time.time;
        normal_Attack();
        updateText();
    }

    public void try_RA()
    {
        if (stamina > 0)
        {
            lastAttack = Time.time;
            ranged_Attack();
            fighterManager.ranged_Attack(gameObject);
            updateText();
        }
    }
}
