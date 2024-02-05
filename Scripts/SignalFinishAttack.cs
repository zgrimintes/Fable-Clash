using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalFinishAttack : MonoBehaviour
{
    public void signalFinishAttack()
    {
        GetComponent<CharacterManager>().animator.SetBool("NA", false);
        GetComponent<CharacterManager>().animator.SetBool("HA", false);
        GetComponent<CharacterManager>().animator.SetBool("RA", false);
        GetComponent<CharacterManager>().animator.SetBool("SA", false);
        GetComponent<CharacterManager>().animator.SetBool("MA", false);
        GetComponent<CharacterManager>().animator.SetBool("Attacking", false);
        GetComponent<CharacterManager>().animator.SetBool("isSpecial", false);
        GetComponent<CharacterManager>().animator.SetBool("notMagic", true);
        GetComponent<AttackManager>().hasHit = true;
        GetComponent<CanHit>().canHit = false;

        if (GetComponent<CharacterManager>()._ch_name == "Capcaunul") GetComponent<CharacterManager>().canDash = true;
    }
}
