using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttacksHarapAlb : MonoBehaviour
{
    public string nameToHave = "HarapAlb"; //for checking if the enemy shoudl inherit this script
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
        checkIfInFront();
        checkHealth();
        checkStamina();
        checkMana();

        if (hasNotAttacked) chooseAttack();
    }

    protected void checkHealth()
    {
        if (playerInstance.GetComponent<CharacterManager>().HP < 5) attacks[3] += .6f;
    }

    protected void checkPreviousAttacks(int attackMade) //For reducing the "spam an attack"
    {
        if (indxAttacks == 0)
        {
            //if (chanceToStartDashing < 1) attacks[4] = 0;
        }
        else if (attackMade == allAttacks[indxAttacks - 1])
        {
            if (indxAttacks >= 2 && allAttacks[indxAttacks - 1] == allAttacks[indxAttacks - 2])
            {
                attacks[attackMade] -= Random.Range(0, .7f);
            }
            else if (Random.Range(0, 4) <= 1) attacks[attackMade] -= .3f;
        }
    }

    protected void checkIfInFront()
    {
        Vector2 startPos = new Vector2(enemyController.attackPoint.position.x, enemyController.attackPoint.position.y);

        if (Physics2D.CircleCast(startPos, 2f, new Vector2(enemyController.horizontalS, 0)) == true) attacks[3] += .2f;
    }

    protected void checkDistance()
    {
        if (!enemyController.isTooFar)
        {
            attacks[0] += .5f; attacks[1] += .5f; attacks[3] += .4f; attacks[2] += .15f; attacks[4] += .15f;
        }
        else
        {
            attacks[3] -= .2f;
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
    }

    protected void checkLookingDir()
    {
        if (enemyController.horizontalS == playerInstance.GetComponent<CharacterManager>().horizontalS)
        {
            attacks[4] += .45f;
            attacks[2] += .3f;
            attacks[3] -= .3f;
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

    private void chooseAttack() //For choosing the optimal attack in that frame
    {
        hasNotAttacked = false;

        float _max_flt = 0;
        int _indx_max = -1;
        checkPreviousAttacks(_indx_max);

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
