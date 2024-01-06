using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReadySignal : MonoBehaviour
{
    public GameObject individualSignal;

    float cooldown;

    private void Awake()
    {
        cooldown = GetComponent<CharacterManager>().cooldown;
    }

    private void Update()
    {
        individualSignal.SetActive(Time.time - GetComponent<CharacterManager>().lastAttack > cooldown);
    }
}
