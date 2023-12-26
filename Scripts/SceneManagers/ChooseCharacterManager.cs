using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoseCharacterManager : MonoBehaviour
{
    public GameObject player, enemy;

    public FighterManager[] fighterManagersP = new FighterManager[9];
    public FighterManager[] fighterManagersE = new FighterManager[9];
    public TextMeshProUGUI countdownText;

    public void characterChoosed(int c)
    {
        /* c is the parameter recievd from teh buttons to indicate witch character is selected
         * first of all positive integers represent the player and ther negative pairs represent the enemy, then we have:
         * 1 - Prislea, 2 - Harap-Alb, 3 - Greuceanul,
         * 4 - Spanul, 5 - Capcaunul, 6 - Zmeul,
         * 7 - Zgripturoaica, 8 - Balaurul, 9 - Crisnicul
         * */

        if (c > 0)
        {
            player.GetComponent<CharacterManager>().fighterManager = fighterManagersP[c - 1];
            player.GetComponent<CharacterManager>().LoadPlayer(fighterManagersP[c - 1]);
            player.GetComponent<AttackManager>().setCharacteristics();
        }
        else
        {
            enemy.GetComponent<CharacterManager>().fighterManager = fighterManagersE[Mathf.Abs(c) - 1];
            enemy.GetComponent<CharacterManager>().LoadPlayer(fighterManagersE[Mathf.Abs(c) - 1]);
            enemy.GetComponent<AttackManager>().setCharacteristics();
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
