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
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D coll;

    int _NA_dmg;
    int _RA_dmg;
    int _HA_dmg;
    int _MA_dmg;
    int _SA_dmg;
    string _ch_name;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        LoadPlayer(fighterManager);
        updateText();
    }

    public void LoadPlayer(FighterManager data) //Function for loading the data from the ScriptableObject into the GameObject
    {
        WeightClass w_data = (WeightClass)data.w_Class;
        speed = w_data.speed;
        _NA_dmg = w_data._NA_dmg;
        _RA_dmg = w_data._RA_dmg;
        _HA_dmg = w_data._HA_dmg;
        _MA_dmg = w_data._MA_dmg;
        _SA_dmg = w_data._S_dmg;

        mana = data.mana;
        HP = data.HP;
        stamina = data.stamina;
        _ch_name = data.characterName;
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
        normal_Attack(_NA_dmg);
        updateText();
    }

    public void try_RA()
    {
        if (stamina > 0)
        {
            lastAttack = Time.time;
            ranged_Attack(_RA_dmg);
            fighterManager.ranged_Attack(gameObject);
            updateText();
        }
    }

    public void try_HA()
    {
        if (stamina > 1)
        {
            lastAttack = Time.time;
            heavy_Attack(_HA_dmg);
            fighterManager.heavy_Attack(gameObject);
            updateText();
        }
    }

    public void try_MA()
    {
        if (mana > 1)
        {
            lastAttack = Time.time;
            magic_Attack(_MA_dmg, _ch_name);
            fighterManager.magic_Attack(gameObject);
            updateText();
        }
    }
}