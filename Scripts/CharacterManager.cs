using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterManager : AttackManager
{
    #region All hidden pubilc variables
    [HideInInspector] public float gravityScale = 4;
    [HideInInspector] public float fallGravityScale = 5;
    [HideInInspector] public int mana;
    [HideInInspector] public int HP;
    [HideInInspector] public int stamina;
    [HideInInspector] public float speed;
    [HideInInspector] public float cooldown;
    [HideInInspector] public float lastAttack;
    [HideInInspector] public float horizontalS; //Save the last looking direction
    [HideInInspector] public bool attackDir; //The direction from where the charater was attacked: FALSE meaning from LEFT and TRUE meaning from RIGHT
    [HideInInspector] public float timeSinceTapped;
    [HideInInspector] public bool doubleTapped = false;
    [HideInInspector] public bool tapped = false;
    [HideInInspector] public float timeToDT = 0.4f;
    [HideInInspector] public int lastKey = 0; // -1 for A and 1 for D
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isKnockback = false;
    [HideInInspector] public bool isGrounded; //To check if the player touches the ground
    [HideInInspector] public Sprite sprite; //To set the sprite of the character
    #endregion

    public FighterManager fighterManager;
    public GameObject textPrfb;
    public LayerMask layer;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D coll;
    [HideInInspector] public GameObject enemy;

    int _NA_dmg;
    int _RA_dmg;
    int _HA_dmg;
    int _MA_dmg;
    int _SA_dmg;
    string _ch_name;

    private float last_mist_dmg;
    private bool hasLost = false;
    private float jumpForce = 21f;
    private float scaleConstant = .3f;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        LoadPlayer(fighterManager);
        updateText();
    }

    protected override void Update()
    {
        base.Update();

        if (transform.localScale.x < 0) horizontalS = -1;
        else horizontalS = 1;

        if (HP <= 0)
        {
            if (hasLost) return;

            hasLost = true;
            enemy.GetComponent<CharacterManager>().fighterManager.roundsWon++;
            GameManager.Instance.updateGameState(GameStates.EndOfRound);
        }
    }

    public void LoadPlayer(FighterManager data) //Function for loading the data from the ScriptableObject into the GameObject
    {
        data.startOfFight();
        WeightClass w_data = (WeightClass)data.w_Class;
        speed = w_data.speed;
        _NA_dmg = w_data._NA_dmg;
        _RA_dmg = w_data._RA_dmg;
        _HA_dmg = w_data._HA_dmg;
        _MA_dmg = w_data._MA_dmg;
        _SA_dmg = w_data._S_dmg;
        cooldown = w_data._Attack_Cooldown;

        mana = data.mana;
        HP = data.HP;
        stamina = data.stamina;
        _ch_name = data.characterName;
        sprite = data.sprite;

        if (!data.enemy) enemy = GameObject.FindGameObjectWithTag("Enemy");
        else enemy = GameObject.FindGameObjectWithTag("Player");

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.transform.localScale = new Vector2(scaleConstant, scaleConstant);
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
    public void Jump()
    {
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, layer);

        if (isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
        }

        if (rb.velocity.y > 0) rb.gravityScale = gravityScale;
        else rb.gravityScale = fallGravityScale;
    }

    public void take_damage(int damage)
    {
        isKnockback = true;
        attackDir = calculateAttackingDir();
        fighterManager.take_damage(gameObject, damage);
        updateText();
    }

    private bool calculateAttackingDir()
    {
        return transform.position.x > enemy.transform.position.x;
    }

    public void OnParticleCollision(GameObject other) //For the damage taken by the mist created by Zmeu's MA
    {
        if (fighterManager.name == "Zmeul") return;

        if (Time.time - last_mist_dmg > 1f)
        {
            last_mist_dmg = Time.time;
            // take_damage(1);
            fighterManager.take_damage(gameObject, 1);
            updateText();
        }
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!tapped) tapped = true;
            else
            {
                if (Time.time - timeSinceTapped < timeToDT && lastKey == 1)
                {
                    doubleTapped = true;
                }

                tapped = false;
            }
            lastKey = 1;
            timeSinceTapped = Time.time;
        } //Dash for right

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!tapped) tapped = true;
            else
            {
                if (Time.time - timeSinceTapped < timeToDT && lastKey == -1)
                {
                    doubleTapped = true;
                }

                tapped = false;
            }
            timeSinceTapped = Time.time;
            lastKey = -1;
        } //Dash for left

        if (doubleTapped)
        {
            doubleTapped = false;
            StartCoroutine(Dashh());
        }
    }

    public IEnumerator Dashh()
    {
        isDashing = true;
        lastKey = 0;
        rb.velocity = new Vector2(horizontalS * speed * 3, rb.velocity.y);
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
    }

    public IEnumerator Knockback()
    {
        yield return new WaitForSeconds(0.1f);
        isKnockback = false;
    }

    public void try_NA()
    {
        if (Time.time - lastAttack < cooldown) return;

        lastAttack = Time.time;
        normal_Attack(_NA_dmg);
        updateText();
    }

    public void try_RA()
    {
        if (Time.time - lastAttack < cooldown) return;

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
        if (Time.time - lastAttack < cooldown) return;

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
        if (Time.time - lastAttack < cooldown) return;

        if (mana >= 2)
        {
            lastAttack = Time.time;
            magic_Attack(_MA_dmg, _ch_name);
            fighterManager.magic_Attack(gameObject);
            updateText();
        }
    }

    public void try_SA()
    {
        if (Time.time - lastAttack < cooldown) return;

        if (mana >= 3)
        {
            lastAttack = Time.time;
            special_Attack(_SA_dmg, _ch_name);
            fighterManager.special_Attack(gameObject);
            updateText();
        }
    }
}