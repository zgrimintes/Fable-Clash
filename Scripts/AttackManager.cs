using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private GameObject Weapon;
    [SerializeField] private Vector3 offset;

    public void Attack()
    {
        GameObject wp = Instantiate(Weapon, transform.position, Quaternion.identity);
        if (GetComponent<PlayerManager>().horizontalS == -1) wp.GetComponent<Animator>().Play("SwordSwingLeft");
        else if (GetComponent<PlayerManager>().horizontalS == 1) wp.GetComponent<Animator>().Play("SwordSwing");

        wp.AddComponent<FollowPlayer>().toFollow = gameObject; //Adding the FollowPlayer script 
    }
}
