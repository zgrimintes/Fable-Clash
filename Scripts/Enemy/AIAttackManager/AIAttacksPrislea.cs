using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttacksPrislea : MonoBehaviour
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
        if (!enemyController.canAttack) return;

        if (Time.time - enemyController.lastAttack < enemyController.cooldown) return;

        attacks[0] = attacks[1] = attacks[2] = attacks[3] = attacks[4] = 0;

        checkDistance();
        checkLookingDir();
        checkYAxis();
        checkMana();
        checkStamina();
        checkVelocity();

        chooseAttack();
    }

    protected void checkVelocity()
    {
        if (playerInstance.GetComponent<Rigidbody2D>().velocity.magnitude > 10f)
        {
            attacks[4] = 0;
        }
    }

    protected void checkDistance()
    {
        if (!enemyController.isTooFar)
        {
            attacks[0] += .5f; attacks[1] += .5f; attacks[3] += .15f; attacks[2] += .15f; attacks[4] += .15f;
        }
        else
        {
            attacks[3] += .4f;
            attacks[2] += .6f; attacks[4] += .5f;
            if (playerInstance.GetComponent<CharacterManager>().HP > 10)
            {
                attacks[3] += .1f;
                attacks[2] += .3f;
                attacks[4] -= .1f;
            }
            else
            {
                attacks[3] += .15f;
                attacks[4] += .3f;
                attacks[2] -= .1f;
            }
        }
    }

    protected void checkLookingDir()
    {
        if (enemyController.horizontalS == playerInstance.GetComponent<CharacterManager>().horizontalS)
        {
            attacks[4] += .35f;
            attacks[2] += .3f;
            attacks[3] += .5f;
        }
        else
        {
            if (attacks[3] != 0) attacks[3] += .2f;
            if (attacks[1] != 0) attacks[1] += .3f;
        }
    }

    protected void checkStamina()
    {
        if (enemyController.stamina == 1) { attacks[2] += .2f; attacks[1] = 0; }
        else if (enemyController.stamina >= 2) { attacks[1] += .2f; attacks[2] -= .1f; }
        else attacks[1] = attacks[2] = 0;
    }

    protected void checkMana()
    {
        if (enemyController.mana == 2) { attacks[3] += .2f; attacks[4] = 0; }
        else if (enemyController.mana >= 3) { attacks[3] += .1f; attacks[4] += .35f; }
        else if (enemyController.mana < 2) attacks[3] = attacks[4] = 0;
    }

    protected void checkYAxis()
    {
        if (Mathf.Abs(playerInstance.transform.position.y) - Mathf.Abs(transform.position.y) > 0.28f)
        {
            attacks[3] -= .2f;
            attacks[2] -= .2f;
            attacks[4] += .1f;
        }
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

        Attack(_indx_max + 1);
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
