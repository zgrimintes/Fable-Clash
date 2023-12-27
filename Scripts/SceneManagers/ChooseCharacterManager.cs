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
    public TextMeshProUGUI infoText;

    public void characterChoosed(int c)
    {
        /* c is the parameter recievd from teh buttons to indicate witch character is selected
         * we have:
         * 1 - Prislea, 2 - Harap-Alb, 3 - Greuceanul,
         * 4 - Spanul, 5 - Capcaunul, 6 - Zmeul,
         * 7 - Zgripturoaica, 8 - Balaurul, 9 - Crisnicul
         * these are available to the enemy selection too
         * */

        if (fighterManagersP[c - 1] != null)
        {
            player.GetComponent<CharacterManager>().fighterManager = fighterManagersP[c - 1];
            player.GetComponent<CharacterManager>().LoadPlayer(fighterManagersP[c - 1]);
            player.GetComponent<AttackManager>().setCharacteristics();
        }

        showInfo(c);
    }

    public void enemyChoosed(int c)
    {
        if (fighterManagersE[c - 1] != null)
        {
            enemy.GetComponent<CharacterManager>().fighterManager = fighterManagersE[c - 1];
            enemy.GetComponent<CharacterManager>().LoadPlayer(fighterManagersE[c - 1]);
            enemy.GetComponent<AttackManager>().setCharacteristics();
        }
        showInfo(c);
    }

    public void startGame()
    {
        if (player.GetComponent<CharacterManager>().fighterManager == null || enemy.GetComponent<CharacterManager>().fighterManager == null) return;

        gameObject.SetActive(false);
        countdownText.GetComponent<Animator>().Play("Countdown");
        countdownText.GetComponentInParent<OffFinghtManager>().startOfFight();
    }

    public void showInfo(int i)
    {
        infoText.text = getInfo(i);
    }

    private string getInfo(int i)
    {
        switch (i)
        {
            case 1:
                return "Prislea (LightWeight)\r\nNA - Bow jab\r\nHa - Bow strike\r\nRA- arrow\r\nMa - Mace trow\r\nSA - arrow rain";
            case 2:
                return "Harap Alb (MediumWeight)\r\nNA - Sword Slash\r\nHA - Sword Thrust\r\nRA - Shield Throw\r\nMA - Bear Roar\r\nSA - Sword Lunge";
            case 6:
                return "Zmeul (HeavyWeight)\r\nNA - Axe Blow\r\nHA - Axe Smash\r\nRA - Axe Throw\r\nMA - Dark Mist\r\nSA - Axe Rush";
            default:
                return "N/A";
        }
    }
}
