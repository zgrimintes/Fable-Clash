using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CharacterManager : AttackManager
{
    #region All hidden pubilc variables
    [HideInInspector] public int mana;
    [HideInInspector] public int HP;
    [HideInInspector] public int stamina;
    [HideInInspector] public int blocks = 3;
    [HideInInspector] public float speed;
    [HideInInspector] public string _ch_name;
    [HideInInspector] public float cooldown;
    [HideInInspector] public float lastAttack;
    [HideInInspector] public float jumpForce = 21f;
    [HideInInspector] public float fallGravityScale = 5;
    [HideInInspector] public float gravityScale = 4;
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
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool canTakeDamage = true;
    [HideInInspector] public bool isDangerous = false; //For attentioning the enemy whenm it should run
    [HideInInspector] public Sprite sprite; //To set the sprite of the character
    [HideInInspector] public Sprite icon; //To set the icon of the character
    [HideInInspector] public bool hasLost = false;
    [HideInInspector] public float timeToGetRidOfEffects;
    #endregion

    public Animator animator;
    public BarsManager healthBar;
    public BarsManager manaBar;
    public BarsManager staminaBar;
    public BarsManager blockBar;
    public GameObject roundsWonText;
    public GameObject popUpTextPrefab;
    public FighterManager fighterManager;
    public GameObject textPrfb;
    public LayerMask layer;
    [HideInInspector] public GameObject enemy;

    [HideInInspector] public float _NA_dmg;
    float _RA_dmg;
    float _HA_dmg;
    float _MA_dmg;
    float _SA_dmg;

    int indxEffects = 0; //For going trough every effect

    [HideInInspector] public bool checkJumping = true;

    private float last_mist_dmg;
    private float scaleConstant = .3f;

    public float[] defaultValues = new float[10]; //For saving the default values of variables ->
                                                  // -> 0 - cooldown; 1 - na; 2 - ra; 3 - ha; 4 - ma; 5 - sa; 6 - speed; 7 - timeToGetRidOfEffects
    private float[] inflictedTime = new float[11]; //For saving the time when the effect was inflicted -^
    private bool[] hasEffects = { false, false, false, false, false, false, false, false, false, false, false }; //For indicating when an individual has effects

    protected override void Start()
    {
        base.Start();

        LoadPlayer(fighterManager);

        updateText();
    }

    public void setMaxValueBars()
    {
        if (healthBar != null) healthBar.setMaxValue(HP);
        if (manaBar != null) manaBar.setMaxValue(mana);
        if (healthBar != null) staminaBar.setMaxValue(stamina);
        if (blockBar != null) blockBar.setMaxValue(blocks);
    }

    protected override void Update()
    {
        base.Update();

        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .01f, layer))
        {
            animator.SetBool("isJumping", false);
            checkJumping = true;
        }
        if (!Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .01f, layer) && checkJumping)
        {
            animator.SetBool("isJumping", true);
        }

        if (transform.localScale.x < 0) horizontalS = -1;
        else horizontalS = 1;

        if (rb.velocity.y > 0) rb.gravityScale = gravityScale;
        else rb.gravityScale = fallGravityScale;

        if (HP <= 0)
        {
            if (hasLost) return;

            hasLost = true;
            stopAllAnims();
            enemy.GetComponent<CharacterManager>().fighterManager.roundsWon++;
            GameManager.Instance.updateGameState(GameStates.EndOfRound);
        }

        getRidOfEffects(indxEffects);
        indxEffects = (indxEffects + 1) % 11;
    }

    public void LoadPlayer(FighterManager data) //Function for loading the data from the ScriptableObject into the GameObject
    {
        data.startOfFight();
        WeightClass w_data = (WeightClass)data.w_Class;
        speed = w_data.speed; defaultValues[6] = speed;
        _NA_dmg = w_data._NA_dmg; defaultValues[1] = _NA_dmg;
        _RA_dmg = w_data._RA_dmg; defaultValues[2] = _RA_dmg;
        _HA_dmg = w_data._HA_dmg; defaultValues[3] = _HA_dmg;
        _MA_dmg = w_data._MA_dmg; defaultValues[4] = _MA_dmg;
        _SA_dmg = w_data._S_dmg; defaultValues[5] = _SA_dmg;
        cooldown = w_data._Attack_Cooldown; defaultValues[0] = cooldown;

        mana = data.mana;
        HP = data.HP;
        stamina = data.stamina;
        _ch_name = data.characterName;
        sprite = data.sprite;
        icon = data.characterIcon;
        timeToGetRidOfEffects = data.timeToGetRidOfEffects; defaultValues[7] = timeToGetRidOfEffects;

        if (gameObject.name == "Player") enemy = GameObject.FindGameObjectWithTag("Enemy");
        else enemy = GameObject.FindGameObjectWithTag("Player");

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.transform.localScale = new Vector2(scaleConstant, scaleConstant);

        Projectile.GetComponent<SpriteRenderer>().sprite = fighterManager.projectile;
        animator.runtimeAnimatorController = fighterManager.animator as RuntimeAnimatorController;

        setMaxValueBars();
    }

    public void updateText() //Sould be called updateBars 
    {
        if (healthBar != null) healthBar.setValue(HP);
        if (manaBar != null) manaBar.setValue(mana);
        if (staminaBar != null) staminaBar.setValue(stamina);
        if (blockBar != null) blockBar.setValue(blocks);

        //There is no use for text since now all stats are shown up on bars
        /*string hName = "HealthP", mName = "ManaP ", sName = "StaminaP";
        if (gameObject.name == "Enemy")
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
        }*/
    }

    public void popUpText(string txt, int amount)
    {
        GameObject checkClone = GameObject.Find("PopUpText(Clone)");
        GameObject clone;

        if (checkClone == null) //Don't write text over another
            clone = Instantiate(popUpTextPrefab, transform.position, Quaternion.identity);
        else clone = Instantiate(popUpTextPrefab, new Vector3(transform.position.x + 2f, transform.position.y, 0), Quaternion.identity);

        clone.GetComponent<SignalTurnBackColor>().character = gameObject;

        switch (txt)
        {
            case "heal":
                clone.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount;
                clone.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                break;
            case "boost":
                if (amount == 3)//HarapAlb's MA
                {
                    clone.GetComponentInChildren<TextMeshProUGUI>().text = "+1 DMG";
                    clone.GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
                }
                else if (amount == 4)//Crisnicu's SA
                {
                    clone.GetComponentInChildren<TextMeshProUGUI>().text = "x2 DMG";
                    clone.GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
                }
                break;
            case "nerf":
                if (amount == 1)//Prislea's MA
                {
                    clone.GetComponentInChildren<TextMeshProUGUI>().text = "+1s cooldown";
                }
                else if (amount == 2) //Zgripturoaica's MA
                {
                    clone.GetComponentInChildren<TextMeshProUGUI>().text = "Slowed!";
                }
                else if (amount == 3) //Capcaunu's MA
                {
                    clone.GetComponentInChildren<TextMeshProUGUI>().text = "-1 DMG";
                }
                else if (amount == 4) //Crisnicu's MA
                {
                    clone.GetComponentInChildren<TextMeshProUGUI>().fontSize = 2;
                    clone.GetComponentInChildren<TextMeshProUGUI>().text = "Drained!";
                }

                clone.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;

                break;
            case "dmg":
                if (amount == 0) { Destroy(clone); return; }

                clone.GetComponentInChildren<TextMeshProUGUI>().text = "-" + amount;
                clone.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.4941176f, 0.4941176f, 1f);
                break;
        }
    }

    public void startFight()
    {
        //Reset all stats
        getRidOfAllEffects();
        fighterManager.startOfFight();
        HP = fighterManager.HP;
        stamina = fighterManager.stamina;
        mana = fighterManager.mana;
        blocks = 3;

        //Update score
        roundsWonText.GetComponent<TextMeshProUGUI>().text = fighterManager.roundsWon.ToString();

        //Update the rest of the stuffs
        updateText();
        hasLost = false;
    }

    public void stopAllAnims()
    {
        GetComponent<SignalFinishAttack>().signalFinishAttack();
        animator.SetFloat("speed", 0);
        animator.SetBool("isJumping", false);
        animator.SetBool("isBlocking", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("Hurt", false);
    }

    public void applyEfects(int effect) //For applying effects inflicted by attacks to characters
    {
        if (!canTakeDamage) return;

        if (effect != 0 && effect != 8 && effect != 9)
        {
            inflictedTime[effect - 1] = Time.time;
            hasEffects[effect - 1] = true;
        }

        switch (effect)
        {
            case 0:
                break;
            case 1:
                popUpText("nerf", 1);
                cooldown += 1f;
                break;
            case 2:
                _NA_dmg *= 2;
                _HA_dmg += 1;
                _RA_dmg += 1;
                break;
            case 3:
                stopAllAnims();
                if (name == "Player") GetComponent<PlayerManager>().canMove = false;
                else GetComponent<EnemyController>().stateMachine.Change(GetComponent<EnemyController>().waitState);
                timeToGetRidOfEffects = 3.5f;
                break;
            case 4:
                _NA_dmg -= 1;
                _HA_dmg -= 1;
                _RA_dmg -= 1;
                popUpText("nerf", 3);
                GetComponent<SpriteRenderer>().color = new Color(.7f, .7f, .7f, 1);
                break;
            case 5:
                fighterManager.mana = mana = 0;
                updateText();
                cooldown += 1.5f;
                speed -= 3.5f;
                timeToGetRidOfEffects += 1f;

                popUpText("nerf", 2);
                GetComponent<SpriteRenderer>().color = new Color(.7f, .7f, .7f, 1);
                break;
            case 6:
                Debug.Log("Shield Up!");
                timeToGetRidOfEffects = 4f;
                canTakeDamage = false;
                break;
            case 7:
                timeToGetRidOfEffects = 5f;
                _NA_dmg *= 2;
                _RA_dmg *= 2;
                _HA_dmg *= 2;
                break;
            case 8:
                Vector3 newPos = new Vector3(enemy.transform.position.x + 3 * enemy.GetComponent<CharacterManager>().horizontalS, enemy.transform.position.y);

                if (GetComponent<MagicAbilitiesManager>().canTeleportTo(newPos))
                {
                    transform.position = newPos;
                }

                break;
            case 9:
                fighterManager.mana = mana = 0;
                fighterManager.stamina = stamina = 0;
                updateText();

                popUpText("nerf", 4);
                break;
            case 10:
                stopAllAnims();
                if (name == "Player") GetComponent<PlayerManager>().canMove = false;
                else GetComponent<EnemyController>().stateMachine.Change(GetComponent<EnemyController>().waitState);
                timeToGetRidOfEffects += .5f;
                break;
        }
    }

    public void getRidOfEffects(int i)
    {
        if (Time.time - inflictedTime[i] > timeToGetRidOfEffects)
        {
            switch (i)
            {
                case 0:
                    if (!hasEffects[0]) return;

                    cooldown = defaultValues[i];

                    hasEffects[0] = false;
                    break;
                case 1:
                    if (!hasEffects[1]) return;

                    _NA_dmg = defaultValues[1];
                    _RA_dmg = defaultValues[2];
                    _HA_dmg = defaultValues[3];
                    timeToGetRidOfEffects = defaultValues[7]; //For HarapAlb's MA

                    hasEffects[1] = false;
                    break;
                case 2:
                    if (!hasEffects[2]) return;

                    if (name == "Player") GetComponent<PlayerManager>().canMove = true;
                    else GetComponent<EnemyController>().stateMachine.Change(GetComponent<EnemyController>().chaseState);
                    timeToGetRidOfEffects = defaultValues[7];

                    //Reset the mana and stamina after "sleep"
                    fighterManager.stamina = 5;
                    fighterManager.mana = 4;
                    stamina = 5;
                    mana = 4;

                    hasEffects[2] = false;
                    break;
                case 3:
                    if (!hasEffects[3]) return;

                    _NA_dmg = defaultValues[1];
                    _RA_dmg = defaultValues[2];
                    _HA_dmg = defaultValues[3];

                    hasEffects[3] = false;
                    break;
                case 4:
                    if (!hasEffects[4]) return;

                    cooldown = defaultValues[0];
                    speed = defaultValues[6];
                    timeToGetRidOfEffects = defaultValues[7];

                    hasEffects[4] = false;
                    break;
                case 5:
                    if (!hasEffects[5]) return;

                    canTakeDamage = true;
                    timeToGetRidOfEffects = defaultValues[7];

                    hasEffects[5] = false;
                    break;
                case 6:
                    if (!hasEffects[6]) return;

                    timeToGetRidOfEffects = defaultValues[7];
                    _NA_dmg = defaultValues[1];
                    _RA_dmg = defaultValues[2];
                    _HA_dmg = defaultValues[3];

                    hasEffects[5] = false;
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    if (!hasEffects[9]) return;

                    if (name == "Player") GetComponent<PlayerManager>().canMove = true;
                    else GetComponent<EnemyController>().stateMachine.Change(GetComponent<EnemyController>().chaseState);
                    timeToGetRidOfEffects = defaultValues[7];

                    hasEffects[9] = false;
                    break;
            }
        }
    }

    public void getRidOfAllEffects()
    {
        for (int i = 0; i < 10; i++)
        {
            switch (i)
            {
                case 0:
                    cooldown = defaultValues[i];
                    break;
                case 1:
                    _NA_dmg = defaultValues[1];
                    _RA_dmg = defaultValues[2];
                    _HA_dmg = defaultValues[3];
                    timeToGetRidOfEffects = defaultValues[7]; //For HarapAlb's MA
                    break;
                case 2:
                    timeToGetRidOfEffects = defaultValues[7];
                    break;
                case 3:
                    _NA_dmg = defaultValues[1];
                    _RA_dmg = defaultValues[2];
                    _HA_dmg = defaultValues[3];
                    break;
                case 4:
                    cooldown = defaultValues[0];
                    speed = defaultValues[6];
                    timeToGetRidOfEffects = defaultValues[7];
                    break;
                case 5:
                    canTakeDamage = true;
                    timeToGetRidOfEffects = defaultValues[7];
                    break;
                case 6:
                    timeToGetRidOfEffects = defaultValues[7];
                    _NA_dmg = defaultValues[1];
                    _RA_dmg = defaultValues[2];
                    _HA_dmg = defaultValues[3];
                    break;
                case 9:
                    timeToGetRidOfEffects = defaultValues[7];
                    break;
            }
            hasEffects[i] = false;
        }
    }

    public void Jump()
    {
        if (!canDash) return; //Don't jump if you can't

        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, layer);

        if (isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
        }
    }

    public async void Block()
    {
        if (blocks < 1) return;
        if (Time.time - lastAttack < cooldown) return;

        lastAttack = Time.time;
        blocks--;
        updateText();

        canTakeDamage = false;
        animator.SetBool("isBlocking", true);
        Debug.Log("Blocks");

        await Task.Delay(800);

        animator.SetBool("isBlocking", false);
        canTakeDamage = true;
    }

    public void take_damage(float damage)
    {
        if (!canTakeDamage) return;

        //Some visual effects
        popUpText("dmg", (int)damage);

        //The logic stuff
        isKnockback = true;
        GetComponent<SignalFinishAttack>().signalFinishAttack(); //Stop the attacks if you are being hit
        animator.SetBool("Hurt", true); //GetComponent<SignalFinishAttack>().signalFinishAttack();
        attackDir = calculateAttackingDir();
        fighterManager.take_damage(gameObject, (int)damage);
        updateText();
    }

    public void groundShake()
    {
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, layer);
        if (isGrounded)
        {
            take_damage(1);
        }
    }

    public void turnBackColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    private bool calculateAttackingDir()
    {
        return transform.position.x > enemy.transform.position.x;
    }

    public void OnParticleCollision(GameObject other) //For the damage taken by the mist created by Zmeu's MA
    {
        if (fighterManager.name == "Zmeul" || fighterManager.name == "ZmeulEnemy") return;

        if (Time.time - last_mist_dmg > 1f)
        {
            last_mist_dmg = Time.time;
            fighterManager.take_damage(gameObject, 1);
            popUpText("dmg", 1);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4941176f, 0.4941176f, 1f);

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
        if (canDash) //Don't dash if the circumstances don't let you
        {
            isDashing = true;
            animator.SetBool("isDashing", true);
            lastKey = 0;
            rb.velocity = new Vector2(horizontalS * speed * 3, rb.velocity.y);
            yield return new WaitForSeconds(0.2f);
            isDashing = false;
            animator.SetBool("isDashing", false);
        }
        else yield return null;
    }

    public IEnumerator Knockback()
    {
        yield return new WaitForSeconds(.4f);
        isKnockback = false;
        animator.SetBool("Hurt", false);
    }

    public void try_NA()
    {
        if (!canDash || isDashing) return;
        if (Time.time - lastAttack < cooldown) return;

        lastAttack = Time.time;
        normal_Attack(_NA_dmg);
        updateText();
    }

    public void try_RA()
    {
        if (!canDash || isDashing) return;
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
        if (!canDash || isDashing) return;
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
        if (!canDash || isDashing) return;
        if (Time.time - lastAttack < cooldown) return;

        if (mana >= 2)
        {
            lastAttack = Time.time;
            magic_Attack(_MA_dmg);
            fighterManager.magic_Attack(gameObject);
            updateText();
        }
    }

    public void try_SA()
    {
        if (!canDash || isDashing) return;
        if (Time.time - lastAttack < cooldown) return;

        if (mana >= 3)
        {
            lastAttack = Time.time;
            special_Attack(_SA_dmg);
            fighterManager.special_Attack(gameObject);
            updateText();
        }
    }
}