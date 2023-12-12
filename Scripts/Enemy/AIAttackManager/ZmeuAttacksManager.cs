using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIAttacksZmeu : MonoBehaviour
{
    public EnemyController enemyController;
    GameObject playerInstance;

    float[] attacks = new float[5];

    public void Start()
    {
        enemyController = GetComponent<EnemyController>();
        playerInstance = enemyController.playerInstance;
    }

    public void Update()
    {
        if (Time.time - enemyController.lastAttack < enemyController.cooldown) return;

        attacks[0] = attacks[1] = attacks[2] = attacks[3] = attacks[4] = 0;

        if (!enemyController.isTooFar)
        {
            attacks[0] += .5f; attacks[1] += .5f; attacks[3] += .5f; attacks[2] += .15f; attacks[4] += .15f;
        }
        else
        {
            attacks[2] += .6f; attacks[4] += .5f;
            if (playerInstance.GetComponent<CharacterManager>().HP > 10)
            {
                attacks[2] += .3f;
                attacks[4] -= .1f;
            }
            else
            {
                attacks[4] += .3f;
                attacks[2] -= .1f;
            }
        }

        if (enemyController.horizontalS == playerInstance.GetComponent<CharacterManager>().horizontalS)
        {
            attacks[4] += .35f;
            attacks[2] += .3f;
            attacks[3] -= .5f;
        }
        else
        {
            if (attacks[3] != 0) attacks[3] += .4f;
            if (attacks[1] != 0) attacks[1] += .3f;
        }

        if (enemyController.stamina == 1) { attacks[2] += .2f; attacks[1] = 0; }
        else if (enemyController.stamina >= 2) { attacks[1] += .2f; attacks[2] -= .1f; }
        else attacks[1] = attacks[2] = 0;

        if (enemyController.mana == 2) { attacks[3] += .3f; attacks[4] = 0; }
        else if (enemyController.mana >= 3) { attacks[3] += .1f; attacks[4] += .15f; }
        else if (enemyController.mana < 2) attacks[3] = attacks[4] = 0;


        Debug.Log(attacks[0] + " " + attacks[1] + " " + attacks[2] + " " + attacks[3] + " " + attacks[4] + " ");
        chooseAttack();
    }

    private void chooseAttack() //For choosing the optimal attack in that frame
    {
        float _max_flt = 0;
        int _indx_max = -1;

        for (int i = 0; i < 5; i++)
        {
            if (attacks[i] > _max_flt)
            {
                _max_flt = attacks[i];
                _indx_max = i;
            }
        }

        Attack(_indx_max);
    }

    private void Attack(int nb) // 1 for NA, 2 for HA, 3 for RA, 4 for MA, 5 for SA
    {
        switch (nb)
        {
            case 1:
                gameObject.GetComponent<CharacterManager>().try_NA();
                break;
            case 2:
                gameObject.GetComponent<CharacterManager>().try_HA();
                break;
            case 3:
                gameObject.GetComponent<CharacterManager>().try_RA();
                break;
            case 4:
                gameObject.GetComponent<CharacterManager>().try_MA();
                break;
            case 5:
                gameObject.GetComponent<CharacterManager>().try_SA();
                break;
        }
    }
}
