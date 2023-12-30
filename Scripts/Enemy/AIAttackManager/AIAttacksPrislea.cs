using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttacksPrislea : MonoBehaviour
{
    public string nameToHave = "Prislea"; //for checking if the enemy shoudl inherit this script
    public EnemyController enemyController;
    GameObject playerInstance;

    float[] attacks = new float[5];
    float[] allAttacks = new float[3000]; //For storing all attacks made by that character
    int indxAttacks = 0;
    bool hasNotAttacked = true;

    public void Start()
    {
        enemyController = GetComponent<EnemyController>();
        playerInstance = enemyController.playerInstance;
    }

    public void OnEnable()
    {
        //Reset the attacks log
        indxAttacks = 0;
        for (int i = 0; i < allAttacks.Length; i++)
        {
            allAttacks[i] = 0;
        }
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

        if (hasNotAttacked) chooseAttack();
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
        if (enemyController.stamina == 1) { attacks[2] += .2f; attacks[1] = -1; }
        else if (enemyController.stamina >= 2) { attacks[1] += .2f; attacks[2] -= .1f; }
        else attacks[1] = attacks[2] = -1;
    }

    protected void checkMana()
    {
        if (enemyController.mana == 2) { attacks[3] += .2f; attacks[4] = -1; }
        else if (enemyController.mana >= 3) { attacks[3] += .1f; attacks[4] += .35f; }
        else if (enemyController.mana < 2) attacks[3] = attacks[4] = -1;
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

    protected void checkPreviousAttacks() //For reducing the "spam an attack"
    {
        if (indxAttacks > 0) //First check if there have been attacks
        {
            if (indxAttacks > 1 && allAttacks[indxAttacks] == allAttacks[indxAttacks - 1]) //Then if the last two attacks were the same reduce the chance of it happening again
                attacks[(int)allAttacks[indxAttacks]] -= Random.Range(0, 1);
            else
                 if (Random.Range(0, 4) <= 1) attacks[(int)allAttacks[indxAttacks]] -= .25f;
        }
    }


    private void chooseAttack() //For choosing the optimal attack in that frame
    {
        hasNotAttacked = false;

        float _max_flt = 0;
        int _indx_max = -1;
        checkPreviousAttacks();

        for (int i = 0; i < 5; i++)
        {
            if (attacks[i] > _max_flt)
            {
                _max_flt = attacks[i];
                _indx_max = i;
            }
        }

        allAttacks[indxAttacks++] = _indx_max;
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

        hasNotAttacked = true;
    }
}
