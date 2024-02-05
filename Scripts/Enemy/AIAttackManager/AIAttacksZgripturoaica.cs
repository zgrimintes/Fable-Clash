using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttacksZgripturoaica : MonoBehaviour
{
    public string nameToHave = "Zgripturoaica"; //for checking if the enemy shoudl inherit this script
    public EnemyController enemyController;
    GameObject playerInstance;

    float[] attacks = new float[5];
    float[] allAttacks = new float[1000]; //For storing all attacks made by that character
    int indxAttacks = 0;
    float chanceToStartPowder;

    public void Start()
    {
        enemyController = GetComponent<EnemyController>();
        playerInstance = enemyController.playerInstance;
    }

    public void OnEnable()
    {
        chanceToStartPowder = Random.Range(0, 3);

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
        //Debug.Log(enemyController.lastAttack); 

        attacks[0] = attacks[1] = attacks[2] = attacks[3] = attacks[4] = 0;

        checkDistance();
        checkLookingDir();
        checkYAxis();
        checkHealt();
        checkEnemyDamage();
        checkForMA();
        checkMana();
        checkStamina();

        chooseAttack();
    }

    protected void checkForMA()
    {
        int att = 12;
        if (enemyController.fighterManager.isBoss) att *= 3;

        for (int i = indxAttacks - 1; i >= 0; i--)
        {
            float toDec = 5; //How much chance it decreases

            if (indxAttacks - i > 12) break;
            if (enemyController.fighterManager.isBoss && indxAttacks - i < att) toDec = .5f;

            if (allAttacks[i] == 3)
            {
                attacks[3] = -toDec;
            }
        }
    }

    protected void checkEnemyDamage()
    {
        if (playerInstance.GetComponent<CharacterManager>()._NA_dmg == playerInstance.GetComponent<CharacterManager>().defaultValues[1]) attacks[3] += .15f;
        else attacks[3] -= .4f;
    }

    protected void checkHealt()
    {
        int HPbd1 = 5, HPbd2 = 7, HPbd3 = 10;

        if (enemyController.fighterManager.isBoss)
        {
            HPbd1 = 15;
            HPbd2 = 40;
            HPbd3 = 50;
        }

        if (enemyController.HP < HPbd1) { attacks[4] += .5f; attacks[3] = -14f; }
        else if (enemyController.HP < HPbd2) attacks[3] -= .9f;
        else if (enemyController.HP > HPbd3) attacks[4] -= .3f;
    }

    protected void checkDistance()
    {
        if (!enemyController.isTooFar)
        {
            attacks[0] += .5f; attacks[1] += .5f; attacks[3] += .1f; attacks[2] += .15f; attacks[4] += .25f;
        }
        else
        {
            attacks[3] += .35f;
            attacks[2] += .5f;
            attacks[4] -= .2f;
        }
    }

    protected void checkLookingDir()
    {
        if (enemyController.horizontalS == playerInstance.GetComponent<CharacterManager>().horizontalS)
        {
            attacks[4] -= .25f;
            attacks[2] += .35f;
            attacks[3] += .3f;
        }
        else
        {
            if (attacks[3] > 0) attacks[3] += .2f;
            if (attacks[1] > 0) attacks[1] += .3f;
        }
    }

    protected void checkStamina()
    {
        if (enemyController.stamina == 1) { attacks[2] += .2f; attacks[1] = -10; }
        else if (enemyController.stamina >= 2) { attacks[1] += .2f; attacks[2] -= .1f; }
        else attacks[1] = attacks[2] = -10;
    }

    protected void checkMana()
    {
        if (enemyController.mana == 2) { attacks[3] += .2f; attacks[4] = -10; }
        else if (enemyController.mana >= 3) { attacks[3] += .1f; attacks[4] += .35f; }
        else if (enemyController.mana < 2) attacks[3] = attacks[4] = -10;
    }

    protected void checkYAxis()
    {
        if (Mathf.Abs(playerInstance.transform.position.y) - Mathf.Abs(transform.position.y) > 0.28f)
        {
            attacks[3] -= .3f;
            attacks[2] -= .2f;
            attacks[4] += .1f;
        }
    }

    protected void checkPreviousAttacks() //For reducing the "spam an attack"
    {
        if (indxAttacks == 0)
        {
            if (chanceToStartPowder <= 1.5f) attacks[3] -= 10;
        }
        else
        {
            if (indxAttacks > 1 && allAttacks[indxAttacks] == allAttacks[indxAttacks - 1]) //Then if the last two attacks were the same reduce the chance of it happening again
                attacks[(int)allAttacks[indxAttacks]] -= Random.Range(0, 1);
            else
                 if (Random.Range(0, 4) <= 1) attacks[(int)allAttacks[indxAttacks]] -= .25f;
        }
    }

    private void chooseAttack() //For choosing the optimal attack in that frame
    {
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

        if (_indx_max != -1) allAttacks[indxAttacks++] = _indx_max;

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

        enemyController.lastAttack = Time.time;
    }
}
