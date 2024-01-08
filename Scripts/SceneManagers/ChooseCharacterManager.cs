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
    public TextMeshProUGUI infoTextP, infoTextNameP, infoTextE, infoTextNameE;

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
            player.GetComponent<PlayerManager>().changeIcon();
        }

        showInfo(c, 'p');
    }

    public void enemyChoosed(int c)
    {
        if (fighterManagersE[c - 1] != null)
        {
            enemy.GetComponent<CharacterManager>().fighterManager = fighterManagersE[c - 1];
            enemy.GetComponent<CharacterManager>().LoadPlayer(fighterManagersE[c - 1]);
            enemy.GetComponent<AttackManager>().setCharacteristics();
            enemy.GetComponent<EnemyController>().choseAIScript();
        }
        showInfo(c, 'e');
    }

    public void startGame()
    {
        if (player.GetComponent<CharacterManager>().fighterManager == null || enemy.GetComponent<CharacterManager>().fighterManager == null) return;

        gameObject.SetActive(false);
        countdownText.GetComponent<Animator>().Play("Countdown");
        countdownText.GetComponentInParent<OffFinghtManager>().startOfFight();
    }

    public void showInfo(int i, char c)
    {
        switch (c)
        {
            case 'p':
                infoTextNameP.text = getName(i);
                infoTextP.text = getInfo(i);
                break;
            case 'e':
                infoTextNameE.text = getName(i);
                infoTextE.text = getInfo(i);
                break;
        }
    }

    private string getName(int i)
    {
        switch (i)
        {
            case 1:
                return "Prislea (LightWeight)";
            case 2:
                return "Harap Alb (MediumWeight)\r\n";
            case 3:
                return "Greuceanu (HeavyWeight)\r\n";
            case 4:
                return "Spinul (LightWeight)\r\n";
            case 5:
                return "Capcaunul (MediumWeight)\r\n";
            case 6:
                return "Zmeul (HeavyWeight)\r\n";
            case 7:
                return "Zgripturoaica (lightWeight)";
            case 8:
                return "Balaurul (MediumWeight)\r\n";
            case 9:
                return "Crisnicul (HeavyWeight)\r\n";
            default:
                return "N/A";
        }
    }

    private string getInfo(int i)
    {
        switch (i)
        {
            case 1:
                return "NA - Bow jab\r\nHa - Bow strike\r\nRA- arrow\r\nMa - Mace trow\r\nSA - arrow rain";
            case 2:
                return "NA - Sword Slash\r\nHA - Sword Thrust\r\nRA - Shield Toss\r\nMA - Bear Roar\r\nSA - Sword Lunge";
            case 3:
                return "NA - Bludgeon Slap\r\nHA - Bludgeon Slam\r\nRA - Sun Blast/Moon Blast\r\nMA - Sleep Bringer\r\nSA - Raven Water Drop";
            case 4:
                return "NA - Sickle Graze\r\nHA - Sickle Gash\r\nRA - Sickle Shy\r\nMA - Shadow Teleport\r\nSA - Sickle Hurricane";
            case 5:
                return "NA - Claw Tear\r\nHA - Claw Hack\r\nRA - Stone Hurl\r\nMA - Deafening Howl\r\nSA - Ground Smash";
            case 6:
                return "NA - Axe Blow\r\nHA - Axe Smash\r\nRA - Axe Throw\r\nMA - Dark Mist\r\nSA - Axe Rush";
            case 7:
                return "NA - Wing Flap\r\nHA - Claw Maul\r\nRA - Wind Slash\r\nMA - Atropa Blow\r\nSA - Wind Shield";
            case 8:
                return "NA - Claws Strike\r\nHA - Hard Bite\r\nRA - Fire Blast\r\nMA - Blazing Boomerang\r\nSA - Fire Breath";
            case 9:
                return "NA - Cleaver Cut\t\nHA - Cleaver Rend\r\nRA - Cleaver Heave\r\nMA - Hexed Pigs\r\nSA - Devil Trust";
            default:
                return "N/A";
        }
    }
}
