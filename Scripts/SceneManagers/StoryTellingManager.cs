using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryTellingManager : MonoBehaviour
{
    public static StoryTellingManager Instance;

    public TMP_FontAsset normalFont, arhaicFont;
    public GameObject storyText1, storyText2, startStoryText;
    public static bool finishedAnimation = false;
    public static bool story = true;
    public static int currentFight;
    public static int nextStoryInstance = 0;
    public static bool[] fightsWon = new bool[9];

    private void Start()
    {
        Instance = this;

        if (startStoryText == null) return;

        if (nextStoryInstance == 0)
        {
            startStoryText.SetActive(true);
        }
        else
        {
            if (!fightsWon[currentFight])
                setFight(currentFight);
            else
            {
                continueButton();
            }
        }
    }

    public void storyStarted()
    {
        story = true;
    }

    public void signalFinishedAnimation(int state)
    {
        finishedAnimation = (state == 1);
    }

    public void continueButton()
    {
        if (!finishedAnimation) return;
        parametersForText();

        storyText1.GetComponent<TextMeshProUGUI>().text = nextStory(++nextStoryInstance);
        storyText2.GetComponent<TextMeshProUGUI>().text = nextStory(++nextStoryInstance);

    }

    public void parametersForText()
    {
        startStoryText.SetActive(false);
        storyText1.SetActive(true);
        storyText2.SetActive(true);

        storyText1.GetComponent<Animator>().enabled = false;
        storyText1.GetComponent<TextMeshProUGUI>().font = normalFont;
        storyText1.GetComponent<TextMeshProUGUI>().fontSize = 28;
        storyText1.transform.localPosition = new Vector2(0, 100);
    }

    public string nextStory(int i)
    {
        switch (i)
        {
            case 1: return "Two realms existed in balance: The Realm of men and the Realm of beasts. \r\nWhen there was sunlight in one of the realms, the other would see the dark of night.";
            case 2: return "This balance was maintained for eons, until suddenly, one day cries could be heard across both realms:";
            case 3: return "\"What’s happening!? The Sun is fading away!\"";
            case 4: return "At the same time, in a far more treacherous land:\r\n\r\n\"The Moon! It’s gone! What curse could this be!?\"";
            case 5: return "The sudden disappearance of the Sun and Moon had sent waves throughout both realms, but the worst of it had yet to come.";
            case 6: return "The future awaits withered forests and dry lands for the inhabitants of these realms.\r\nSick and weak cattle unfit to be food, death and destruction was all that these realms were destined for.\r\n";
            case 7: return "Far away, in a dimly lit room, The Crasnic delighted by these events exclaims:\r\n\"Perfect! The conditions for my plan have been met, now it’s time for me to act on it.\"";
            case 8: return "His acolyte then said: \r\n \"Master, I have linked the realms together for a short time, you may pass on your message.\"";
            case 9:
                storyText1.transform.localPosition = new Vector2(0, 0);
                return "The Crasnic, delighted by these news turns around to face the smoke cloud created by his acolyte and said:\r\n" +
                    "\"Hear ye hear ye, I am a wizard from a distant land, and the despair of your people has moved me, I have decided to help you!\" \r\n" +
                    "\"There exists a Golden Apple with the power of saving one of the realms, but only the worthy will be allowed to use it’s power.\" \r\n" +
                    "Each realm must send forth their strongest warriors to fight over the right to use it’s power, prove your worthiness in a fight!.\" \r\n" +
                    "\"To join this fight, the strongest Men must stand atop of the highest mountain, and the strongest Beasts must stand below the deepest depths, only then will the fight truly start!\"";
            case 10: return "";
            case 11:
                storyText1.transform.localPosition = new Vector2(0, 100);
                return "The words are heard across both realms, as if the heavens themselves were speaking them, and with no other option left, desperately, each realm chose three warriors to represent them.\r\n";
            case 12: return "The realm of Men chose: Praslea, Harap-Alb and Greuceanu.\r\n\r\nThe realm of Beasts chose: Zmeul, Spanul, Capcaunul.\r\n";
            case 13: return "The warriors all easily reached the peak and depth respectively, the hardships of nature were no match for them. When Harap-Alb had finally reached the peak, only moments after Praslea and Greuceanu, the same voice was heard from the heavens:";
            case 14: return "\"Let the Tournament of the Golden Apple begin! The fights will be arranged in sets of three, each one of you facing one opponent! The first to win two of the three fights will be crowned the winner of the fight! Any and all tiebreakers will be decided with a final showdown. Now, let the fighting commence!\"";
            case 15:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "1st fight:";
            case 16:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Praslea vs Zmeul\r\n";
            case 17:
                setFight(1);
                return "";
            case 18: return "";
            case 19:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Prislea has won his fight! Now it is time for the others to do so.";
            case 20: return "";
            case 21:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "2nd fight:";
            case 22:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Harap-Alb vs Spinul\r\n";
            case 23:
                setFight(2);
                return "";
            case 24: return "";
            case 25:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Harap-Alb has won his fight too! Greuceanu's fight comes next.";
            case 26: return "";
            case 27:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "3rd fight:";
            case 28:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Harap-Alb vs Spinul\r\n";
            case 29:
                setFight(3);
                return "";
            case 30: return "";

            default:
                return "N/A";
        }
    }

    public void setFight(int fightNb)
    {
        currentFight = fightNb;
        gameObject.SetActive(false);
        if (fightNb == 1)
        {
            ChoseCharacterManager.instance.characterChoosed(1);
            ChoseCharacterManager.instance.enemyChoosed(6);
        }
        else if (fightNb == 2)
        {
            ChoseCharacterManager.instance.characterChoosed(2);
            ChoseCharacterManager.instance.enemyChoosed(4);
        }
        else if (fightNb == 3)
        {
            ChoseCharacterManager.instance.characterChoosed(3);
            ChoseCharacterManager.instance.enemyChoosed(5);
        }

        ChoseCharacterManager.instance.startGame();
    }
}
