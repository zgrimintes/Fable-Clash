using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoseCharacterManager : MonoBehaviour
{
    public static ChoseCharacterManager instance;

    public TMP_FontAsset fontToChange;
    public GameObject player, enemy;
    public GameObject popUpTextPrefab;

    public FighterManager[] fighterManagersP = new FighterManager[9];
    public FighterManager[] fighterManagersE = new FighterManager[9];
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI infoTextP, infoTextNameP, infoTextE, infoTextNameE;
    public TextMeshProUGUI AbilitiesInfoNameP, AbilitiesInfoNameE, MAInfoP, MAInfoE, SAInfoP, SAInfoE;

    private void Start()
    {
        instance = this;
    }

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

        if (!StoryTellingManager.story) showInfo(c, 'p');
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
        if (!StoryTellingManager.story) showInfo(c, 'e');
    }

    public void startGame()
    {
        if (player.GetComponent<CharacterManager>().fighterManager.characterName == "NULL" || enemy.GetComponent<CharacterManager>().fighterManager.characterName == "NULL")
        {
            GameObject clone = Instantiate(popUpTextPrefab, new Vector2(0, 2.5f), Quaternion.identity);
            clone.GetComponentInChildren<TextMeshProUGUI>().text = "Choose both characters!";
            clone.GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
            clone.GetComponentInChildren<TextMeshProUGUI>().fontSize = 2;

            CameraShake.Shake(.2f, .1f);

            return;
        }

        gameObject.SetActive(false);
        countdownText.GetComponent<Animator>().Play("Countdown");
        countdownText.GetComponentInParent<OffFinghtManager>().startOfFight(true);
    }

    public void showInfo(int i, char c)
    {
        switch (c)
        {
            case 'p':
                if (i == 9) MAInfoP.fontSize = 11f;
                else MAInfoP.fontSize = 12;

                AbilitiesInfoNameP.text = getName(i + 10);
                MAInfoP.text = "Magic Ability: " + getMAInfo(i);
                SAInfoP.text = "Special Ability: " + getSAInfo(i);
                infoTextNameP.text = getName(i);
                infoTextNameP.font = fontToChange;
                infoTextP.font = fontToChange;
                infoTextP.text = getInfo(i);
                break;
            case 'e':
                if (i == 9) MAInfoE.fontSize = 11f;
                else MAInfoE.fontSize = 12;

                AbilitiesInfoNameE.text = getName(i + 10);
                MAInfoE.text = "Magic Ability: " + getMAInfo(i);
                SAInfoE.text = "Special Ability: " + getSAInfo(i);
                infoTextNameE.text = getName(i);
                infoTextNameE.font = fontToChange;
                infoTextE.font = fontToChange;
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
            case 11: return "Prislea \r\n(LightWeight)";
            case 12: return "Harap Alb \r\n(MediumWeight)\r\n";
            case 13: return "Greuceanu \r\n(HeavyWeight)\r\n";
            case 14: return "Spinul \r\n(LightWeight)\r\n";
            case 15: return "Capcaunul \r\n(MediumWeight)\r\n";
            case 16: return "Zmeul \r\n(HeavyWeight)\r\n";
            case 17: return "Zgripturoaica \r\n(lightWeight)";
            case 18: return "Balaurul \r\n(MediumWeight)\r\n";
            case 19: return "Crisnicul \r\n(HeavyWeight)\r\n";
            default:
                return "N/A";
        }
    }

    private string getInfo(int i)
    {
        switch (i)
        {
            case 1:
                return "NA - Bow jab\r\nHa - Bow strike\r\nRA - arrow\r\nMa - Mace trow\r\nSA - arrow rain";
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

    private string getMAInfo(int i)
    {
        switch (i)
        {
            case 1:
                return "Prislea's intelligence helped him in making a weapon that can deal damage and raise one's attack cooldown.";
            case 2:
                return "By winning the bear pelt from his father, HarapAlb's bear roar gives him strenght and increases his damage by one.";
            case 3:
                return "Using his magical knowledge, Greuceanu inflicts a state of sleep that paralyzes the enemy but recharges his mana and stamina.";
            case 4:
                return "Spinu's trickery is clearly present here. He teleports behind the enemy if possible and deals plus one damage to his next attack.";
            case 5:
                return "Capcaunul was a feared creature. His Deafening Howl frightens his enemyes and lowers their damage by one.";
            case 6:
                return "The mist created by the Zmeu is the result of experience in tormenting men. It deals one damage for each second spent in it.";
            case 7:
                return "Atropa, also known as deadly nightshade, is famous for it's toxicity. Even one blow can lower your speed and raise your attack cooldown.";
            case 8:
                return "His fire mastery is like no other's. Balaurul can create a boomerang that travels the screen four times and deals two damage upon impact.";
            case 9:
                return "Crisnicu's hexed pigs will inflict, random, one of three effects: 1.Teleport the enemy in front of him. 2.Drain all his mana and stamina. 3.Paralyze the enemy for 2 seconds.";
            default:
                return "N/A";
        }
    }

    private string getSAInfo(int i)
    {
        switch (i)
        {
            case 1:
                return "Arrows are his best friends so why not make good use of them? The enemy will be surprised by 7 falling arrows each dealing one damage.";
            case 2:
                return "The lunge is the evidence of his courage. Harap Alb will jump and deal four damage upon falling to the ground.";
            case 3:
                return "Greuceanu is the one people always rely on, therefore his healing will restore five hit points in order for him to continue the fight.";
            case 4:
                return "His enemies will be frightened when they'll see Spinu's rotating ability. For 5 seconds if they get close to him one damage will be dealt to them.";
            case 5:
                return "Blinded by anger, Capcaunul will do four gorund shaking hits. If his enemy happens to be on the ground they'll take one damage.";
            case 6:
                return "Manace is his second name. Zmeul won't stop dashing head-first until he hits something dealing four damage to it.";
            case 7:
                return "Zgripturoaica has found a way to use the wind to her advantage. The shield will last four seconds and it will prevent all damage.";
            case 8:
                return "By breathing fire, Balaurul will show his dominance and fierce. Not only that but it will burn anyone that stands in front of it, dealing 2 damage.";
            case 9:
                return "His desire to rule over every realm will show Crisnicu's enemies that they have no chance of winning against his double damage.";
            default:
                return "N/A";
        }
    }

    public void pickMatch()
    {
        int Rch = Random.Range(1, 10), Ren = Random.Range(1, 10);

        if (Rch == 10) Rch = 9;
        if (Ren == 10) Ren = 9;

        characterChoosed(Rch);
        enemyChoosed(Ren);
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
