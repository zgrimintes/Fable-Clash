using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoseCharacterManager : MonoBehaviour
{
    GameObject player, enemy;

    public FighterManager[] fighterManagers = new FighterManager[9];
    public TextMeshProUGUI countdownText;

    private void Start()
    {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
    }

    public void characterChoosed(int c)
    {
        /* c is the parameter recievd from teh buttons to indicate witch character is selected
         * first of all positive integers represent the player and ther negative pairs represent the enemy, then we have:
         * 1 - Prislea, 2 - Harap-Alb, 3 - Greuceanul,
         * 4 - Spanul, 5 - Capcaunul, 6 - Zmeul,
         * 7 - Zgripturoaica, 8 - Balaurul, 9 - Crisnicul
         * */

        switch (c)
        {
            case 1:
                player.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
            case -1:
                enemy.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
            case 6:
                player.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
            case -6:
                enemy.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
        }
    }

    public void startGame()
    {
        if (player.GetComponent<CharacterManager>().fighterManager == null || enemy.GetComponent<CharacterManager>().fighterManager == null) return;

        gameObject.SetActive(false);
        countdownText.GetComponent<Animator>().Play("Countdown");
        countdownText.GetComponentInParent<OffFinghtManager>().startOfFight();
    }
}
