using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalFinishAttack : MonoBehaviour
{
    public void signalFinishAttack()
    {
        GetComponent<CharacterManager>().animator.SetBool("NA", false);
    }
}
