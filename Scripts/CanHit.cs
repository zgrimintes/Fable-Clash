using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Animation Event to toggle the hit abillity
public class CanHit : MonoBehaviour
{
    public bool canHit;

    public void changeHitState()
    {
        canHit = !canHit;
    }
}
