using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    MagicAbilitiesManager magicAbilitiesManager;
    SpecialAttacksManager specialAttacksManager;
    [SerializeField] private GameObject Weapon;
    [SerializeField] private GameObject Projectile;

    [HideInInspector] public GameObject wp;
    [HideInInspector] public bool hasHit; //So you can't hit more than once per attack
    [HideInInspector] public bool normalNextAttack = true;
    [HideInInspector] public float dmg;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D coll;

    public Transform attackPoint;
    public float attackRange = 0.6f;
    public float flying_speed = 0.2f;
    public LayerMask enemyLayer;
    Animator animator;

    private Vector3 positionToAttack;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        magicAbilitiesManager = GetComponent<MagicAbilitiesManager>();
        specialAttacksManager = GetComponent<SpecialAttacksManager>();

        setCharacteristics();
    }

    public void setCharacteristics()
    {
        attackPoint.transform.localPosition = Vector3.zero; //Reset the position of the attack point

        switch (GetComponent<CharacterManager>().fighterManager.w_Class.name)
        {
            case "LightWeight":
                positionToAttack = new Vector2(1.9f, 0);
                coll.size = new Vector2(7.15f, 12.42f);
                coll.offset = new Vector2(-0.63f, 0.10f);
                break;
            case "MediumWeight":
                positionToAttack = new Vector2(5.9f, 0);
                coll.size = new Vector2(10.77f, 16.08f);
                coll.offset = new Vector2(-0.01f, 0.01f);
                break;
            case "HeavyWeight":
                positionToAttack = new Vector2(9.9f, 0);

                if (GetComponent<CharacterManager>()._ch_name == "Greuceanul") //Due to uneven proportions Greuceanu has a different hitbox
                {
                    coll.size = new Vector2(16.63f, 16.99f);
                    coll.offset = new Vector2(1.02f, -0.06f);
                    break;
                }

                coll.size = new Vector2(18.09f, 16.30f);
                coll.offset = new Vector2(1.75f, 0.28f);
                break;
        }

        attackPoint.transform.localPosition += positionToAttack;
    }

    protected virtual void Update()
    {
        if (wp == null)
        {
            hasHit = false;
            return;
        }

        if (wp.GetComponent<CanHit>() == null) return;

        if (wp.GetComponent<CanHit>().canHit && !hasHit)
        {
            checkForColls(attackPoint.position, attackRange, 0);
        }
    }

    public void normal_Attack(float _NA_dmg)
    {
        if (normalNextAttack) dmg = _NA_dmg;

        wp = Instantiate(Weapon, transform.position, Quaternion.identity);
        animator = wp.GetComponent<Animator>();
        wp.AddComponent<FollowPlayer>().toFollow = gameObject; //Adding the FollowPlayer

        if (GetComponent<CharacterManager>().horizontalS == -1) animator.Play("SwordSwingLeft");
        else if (GetComponent<CharacterManager>().horizontalS == 1) animator.Play("SwordSwing");
    }

    public void ranged_Attack(float _RA_dmg)
    {
        if (normalNextAttack) dmg = _RA_dmg;
        float dir = GetComponent<CharacterManager>().horizontalS;

        if (dir >= 0) wp = Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, 270));
        else wp = Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, 90));

        StartCoroutine(ranged(dir, 0));
    }

    public void heavy_Attack(float _HA_dmg)
    {
        if (normalNextAttack) dmg = _HA_dmg;
        wp = Instantiate(Weapon, transform.position, Quaternion.identity);
        animator = wp.GetComponent<Animator>();
        wp.AddComponent<FollowPlayer>().toFollow = gameObject; //Adding the FollowPlayer

        if (GetComponent<CharacterManager>().horizontalS == -1) animator.Play("HeavySwordSwingLeft");
        else if (GetComponent<CharacterManager>().horizontalS == 1) animator.Play("HeavySwordSwing");

    }

    public void magic_Attack(float _MA_dmg, string _ch_name)
    {
        if (normalNextAttack) dmg = _MA_dmg;

        switch (_ch_name)
        {
            case "Prislea":
                float dir = GetComponent<CharacterManager>().horizontalS;
                magicAbilitiesManager.Praslea_MA(wp);
                StartCoroutine(ranged(dir, 1));
                break;
            case "Zmeul":
                magicAbilitiesManager.Zmeul_MA();
                break;
            case "HarapAlb":
                magicAbilitiesManager.HarapAlb_MA();
                break;
            case "Spinul":
                magicAbilitiesManager.Spinul_MA();
                break;
            case "Greuceanul":
                magicAbilitiesManager.Greuceanul_MA();
                break;
        }
    }

    public void special_Attack(float _SA_dmg, string _ch_name)
    {
        if (normalNextAttack) dmg = _SA_dmg;

        switch (_ch_name)
        {
            case "Prislea":
                specialAttacksManager.Praslea_SA(wp, Projectile);
                break;
            case "Zmeul":
                specialAttacksManager.Zmeul_SA(GetComponent<CharacterManager>().horizontalS);
                break;
            case "HarapAlb":
                specialAttacksManager.HarapAlb_SA(gameObject);
                break;
            case "Spinul":
                dmg = 1;
                specialAttacksManager.Spinul_SA();
                break;
            case "Greuceanul":
                specialAttacksManager.Greuceanul_SA();
                break;
        }
    }

    public IEnumerator ranged(float dir, int effect)
    {
        while (Physics2D.OverlapBox(wp.transform.position, wp.transform.localScale, 0, enemyLayer) == null && !outOfBounds(wp))
        {
            wp.transform.position = wp.transform.position + new Vector3(flying_speed * dir * Time.deltaTime, 0, 0);
            yield return null;
        }

        checkForColls(wp.transform.position, 1f, effect);

        Destroy(wp);
    }

    public bool outOfBounds(GameObject gameObj)
    {
        if (gameObj.transform.position.x > 12f || gameObj.transform.position.x < -12f) return true;

        return false;
    }

    public void checkForColls(Vector2 point, float radius, int effect)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(point, radius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharacterManager>().take_damage(dmg);
            enemy.GetComponent<CharacterManager>().applyEfects(effect);
            hasHit = true;
            normalNextAttack = true;
        }
    }
}