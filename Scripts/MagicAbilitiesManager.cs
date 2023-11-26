using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicAbilitiesManager : MonoBehaviour
{
    public GameObject Mace;
    Animator animator;
    AttackManager attackManager;

    private void Start()
    {
        attackManager = GetComponent<AttackManager>();
    }

    public void Praslea_MA(GameObject wp)
    {
        wp = Instantiate(Mace, transform.position, Quaternion.identity);

        animator = wp.GetComponent<Animator>();

        attackManager.wp = wp;
    }
}
