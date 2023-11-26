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
    [SerializeField] private GameObject Weapon;
    [SerializeField] private GameObject Projectile;

    [HideInInspector] public GameObject wp;
    [HideInInspector] public bool hasHit; //So you can't hit more than once per attack

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float flying_speed = 0.2f;
    public LayerMask enemyLayer;
    Animator animator;
    private int dmg;

    protected virtual void Start()
    {
        magicAbilitiesManager = GetComponent<MagicAbilitiesManager>();
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
            checkForColls(attackPoint.position, attackRange);
        }
    }

    public void normal_Attack(int _NA_dmg)
    {
        dmg = _NA_dmg;
        wp = Instantiate(Weapon, transform.position, Quaternion.identity);
        animator = wp.GetComponent<Animator>();
        wp.AddComponent<FollowPlayer>().toFollow = gameObject; //Adding the FollowPlayer

        if (GetComponent<PlayerManager>().horizontalS == -1) animator.Play("SwordSwingLeft");
        else if (GetComponent<PlayerManager>().horizontalS == 1) animator.Play("SwordSwing");
    }

    public void ranged_Attack(int _RA_dmg)
    {
        dmg = _RA_dmg;
        float dir = GetComponent<PlayerManager>().horizontalS;

        if (dir >= 0) wp = Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, 270));
        else wp = Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, 90));

        StartCoroutine(ranged(dir));
    }

    public void heavy_Attack(int _HA_dmg)
    {
        dmg = _HA_dmg;
        wp = Instantiate(Weapon, transform.position, Quaternion.identity);
        animator = wp.GetComponent<Animator>();
        wp.AddComponent<FollowPlayer>().toFollow = gameObject; //Adding the FollowPlayer

        if (GetComponent<PlayerManager>().horizontalS == -1) animator.Play("HeavySwordSwingLeft");
        else if (GetComponent<PlayerManager>().horizontalS == 1) animator.Play("HeavySwordSwing");

        checkForColls(attackPoint.position, attackRange + 0.1f);
    }

    public void magic_Attack(int _MA_dmg, string ch_name)
    {
        dmg = _MA_dmg;

        switch (ch_name)
        {
            case "Praslea":
                float dir = GetComponent<PlayerManager>().horizontalS;
                magicAbilitiesManager.Praslea_MA(wp);
                StartCoroutine(ranged(dir));
                break;
        }
    }

    public IEnumerator ranged(float dir)
    {
        while (Physics2D.OverlapBox(wp.transform.position, wp.transform.localScale, 0, enemyLayer) == null && !outOfBounds(wp))
        {
            wp.transform.position = wp.transform.position + new Vector3(flying_speed * dir, 0, 0);
            yield return null;
        }

        checkForColls(wp.transform.position, 1f);

        Destroy(wp);
    }

    public bool outOfBounds(GameObject gameObj)
    {
        if (gameObj.transform.position.x > 14f || gameObj.transform.position.x < -14f) return true;

        return false;
    }

    public void checkForColls(Vector2 point, float radius)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(point, radius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharacterManager>().take_damage(dmg);
            hasHit = true;
        }
    }
}