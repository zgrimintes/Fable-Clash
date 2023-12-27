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
         * we have:
         * 1 - Prislea, 2 - Harap-Alb, 3 - Greuceanul,
         * 4 - Spanul, 5 - Capcaunul, 6 - Zmeul,
         * 7 - Zgripturoaica, 8 - Balaurul, 9 - Crisnicul
         * these are available to the enemy selection too
         * */

        player.GetComponent<CharacterManager>().fighterManager = fighterManagersP[c - 1];
        player.GetComponent<CharacterManager>().LoadPlayer(fighterManagersP[c - 1]);
        player.GetComponent<AttackManager>().setCharacteristics();
    }

    public void enemyChoosed(int c)
    {
        enemy.GetComponent<CharacterManager>().fighterManager = fighterManagersE[c - 1];
        enemy.GetComponent<CharacterManager>().LoadPlayer(fighterManagersE[c - 1]);
        enemy.GetComponent<AttackManager>().setCharacteristics();
    }

    public void startGame()
    {
        if (player.GetComponent<CharacterManager>().fighterManager == null || enemy.GetComponent<CharacterManager>().fighterManager == null) return;

        gameObject.SetActive(false);
        countdownText.GetComponent<Animator>().Play("Countdown");
        countdownText.GetComponentInParent<OffFinghtManager>().startOfFight();
    }
}
