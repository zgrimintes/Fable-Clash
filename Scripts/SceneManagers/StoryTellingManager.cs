using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryTellingManager : MonoBehaviour
{
    public static StoryTellingManager Instance;

    public TMP_FontAsset normalFont, arhaicFont;
    public GameObject storyText1, storyText2, startStoryText, bTMMButton, cButton;
    public static bool finishedAnimation = false;
    public static bool story = false;
    public static bool bossBattle = false;
    public static int currentFight;
    public static int nextStoryInstance = 0;
    public static bool[] fightsWon = new bool[10];

    private void Start()
    {
        Instance = this;

        if (startStoryText == null) return;

        if (nextStoryInstance == 0 || nextStoryInstance == 44 || nextStoryInstance == 68)
        {
            if (nextStoryInstance == 0) startStoryText.SetActive(true);
            else if (nextStoryInstance == 44)
            {
                storyText1.GetComponent<TextMeshProUGUI>().text = "Chapter 2";
            }
            else if (nextStoryInstance == 68)
            {
                storyText1.GetComponent<TextMeshProUGUI>().text = "Chapter 3";
            }
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

        bTMMButton.SetActive(false);
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

        if (nextStoryInstance < 86)
        {
            storyText1.GetComponent<TextMeshProUGUI>().text = nextStory(++nextStoryInstance);
            storyText2.GetComponent<TextMeshProUGUI>().text = nextStory(++nextStoryInstance);
        }
        else
        {
            //Load credits
        }

    }

    public void parametersForText()
    {
        startStoryText.SetActive(false);
        storyText1.SetActive(true);
        storyText2.SetActive(true);

        storyText1.GetComponent<TextMeshProUGUI>().color = Color.white;
        storyText2.GetComponent<TextMeshProUGUI>().color = Color.white;
        storyText1.GetComponent<Animator>().enabled = false;
        storyText1.GetComponent<TextMeshProUGUI>().font = normalFont;
        storyText2.GetComponent<TextMeshProUGUI>().font = normalFont;
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
            case 12: return "The realm of Men chose: Prislea, Harap-Alb and Greuceanu.\r\n\r\nThe realm of Beasts chose: Zmeul, Spinul, Capcaunul.\r\n";
            case 13: return "The warriors all easily reached the peak and depth respectively, the hardships of nature were no match for them. When Harap-Alb had finally reached the peak, only moments after Prislea and Greuceanu, the same voice was heard from the heavens:";
            case 14: return "\"Let the Tournament of the Golden Apple begin! The fights will be arranged in sets of three, each one of you facing one opponent! The first to win two of the three fights will be crowned the winner of the fight! Any and all tiebreakers will be decided with a final showdown. Now, let the fighting commence!\"";
            case 15:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "1st fight:";
            case 16:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Prislea vs Zmeul\r\n";
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
                return "Greuceanu vs Capcaunul\r\n";
            case 29:
                setFight(3);
                return "";
            case 30: return "";
            case 31:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "And with this final win, the Realm of Men won the tournament.";
            case 32: return "\"Incredible! Delightful! With such a showing I am more than honored to present the Golden Apple to our winner, Prislea! \r\nCongratulations boy, you may now make your wish!\"";
            case 33: return "Finally getting his hands on the Golden Apple, Prislea could feel a macabre aura emanating from it, but pushed it to the back of his head, in order to save his realm. \r\nWhile holding the Golden Apple above his head, Prislea shouted:\r\n";
            case 34: return "\"I wish for my realm to see nothing but safety and comfort for the rest of eternity!\"\r\n";
            case 35: return "As the Golden Apple disappeared into an explosion of light, Prislea and the rest of the inhabitants of the realm of Men awaited their reward patiently, when suddenly a splitting headache launches all of them into grief.";
            case 36: return "As Prislea could feel himself losing his grasp on reality, he realized what was truly happening:\r\n\"The Wizard…it’s his fault!\" The boy shouted, as he was dragged into a smoke cloud.\r\n";
            case 37: return "In the smoke, The Crasnic now had hold over the inhabitants of the realm of Men, the Golden Apple was a lie created to lure in hopeless warriors:\r\n";
            case 38: return "\"Aha! Incredible, even I didn’t expect this to go so smoothly, all that’s left now is to conquer the Realm of Beasts, only then will I truly be unstoppable!\"\r\n";
            case 39: return "Back in the realm of Beasts, the warriors who had just witnessed this were shocked.";
            case 40: return "Zmeul exclaimed: \"What happened!? Why is the realm of Men still withering away? They won!\"\r\n";
            case 41: return "Spinul added: \"This is not good… I have a bad feeling about this, should we do something?\"\r\n";
            case 42: return "Lastly, Capcaunul said: \"This must be related to that Wizard, I bet we’re next, we have to stop him!\"";
            case 43: return "As the three of them ventured into the smoke cloud left behind by The Crasnic, they were now face to face with the warriors that they fought with moments before, except this time they had a stench of evil emanating from them...";
            case 44:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                ChapterSelectManager.CH2 = true;
                return " - End of Chapter 1 - ";
            case 45:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "4th fight:";
            case 46:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Zmeul vs Evil Prislea\r\n";
            case 47:
                setFight(4);
                return "";
            case 48: return "";
            case 49:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Zmeul managed to get Prislea out of Crisnic's hypnosis.";
            case 50: return "";
            case 51:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "5th fight:";
            case 52:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Spinul vs Evil Harap-Alb\r\n";
            case 53:
                setFight(5);
                return "";
            case 54: return "";
            case 55:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Spinul helped Harap-Alb to escape the mind prison. \r\nNow Capcaunul is the last to take care of Greuceanu.";
            case 56: return "";
            case 57:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "6th fight:";
            case 58:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Capcaunul vs Evil Greuceanul\r\n";
            case 59:
                setFight(6);
                return "";
            case 60: return "";
            case 61:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "Lastly Capcaunul did his job by waking Greuceanu up.";
            case 62: return "";
            case 63: return "As the mind-controlled warriors get defeated, The Crasnic lost control over them, and although the Realms of Men and Beasts are enemies, the warriors speak as such:\r\n";
            case 64: return "\"Ugh, this guy, I think he’s trying to destroy all of us!\", said Prislea\r\nHarap-Alb continued: \"You don’t say! I can barely feel my legs! \"";
            case 65: return "Suddenly, Spinul said:\r\n\"I know our realms don’t get along, but we have to team up to stop this guy, we’re all going to die if we don’t!\"";
            case 66: return "Spinul’s words are interrupted by The Crasnic:";
            case 67: return "\"If you don’t? You’ll die anyway! Zgripturoaica, Balaurul, finish them! Do not allow them to leave this place!\"";
            case 68:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                ChapterSelectManager.CH3 = true;
                return " - End of Chapter 2 - ";
            case 69:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "7th fight:";
            case 70:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "The Realm of Men vs Zgripturoaica";
            case 71:
                setFight(7);
                return "";
            case 72: return "";
            case 73:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "8th fight:";
            case 74:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "The Realm of Beasts vs Balaurul";
            case 75:
                setFight(8);
                return "";
            case 76: return "";
            case 77:
                return "After both of Crisnicu's acolytes were defeated he appeared and yelled: \r\n\"How dare you be such weaklings! Get out of my face!\"";
            case 78:
                return "With those words, and with a swing of his cleaver, The Crasnic executes both Zgripturoaica and Balaurul, turning his attention to the group of six people, now standing before him:\r\n\"I’ll take care of you myself! After I’m done with you I’ll take over your realms. Don’t say I didn’t warn you!\"";
            case 79:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                storyText1.GetComponent<TextMeshProUGUI>().color = Color.red;
                return "9th fight:";
            case 80:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                storyText2.GetComponent<TextMeshProUGUI>().color = Color.red;
                return "The Warriors of both realms vs The Crasnic";
            case 81:
                setFight(9);
                return "";
            case 82: return "";
            case 83:
                storyText1.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "After The Crasnic’s defeat his body dissipates, the Sun and Moon erupting from his stomach returning to their rightful place. \r\n" +
                    "With the crisis averted and the balance reinstituted, the warriors each returned to their home realms to live untold stories, stories for another time...";
            case 84:
                storyText2.GetComponent<TextMeshProUGUI>().font = arhaicFont;
                return "- End of Fable Clash -";
            case 85:
                bTMMButton.SetActive(true);
                cButton.GetComponentInChildren<TextMeshProUGUI>().text = "Credits >";
                return "Thank you all for playing! \r\nWe hope that you had a great time playing our game!\r\n";
            case 86:
                storyText2.GetComponent<TextMeshProUGUI>().fontSize = 18;
                return "You can alwasy go to the Freeplay and continue playing as long as you want.";
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
        else if (fightNb == 4)
        {
            ChoseCharacterManager.instance.characterChoosed(6);
            ChoseCharacterManager.instance.enemyChoosed(1);
        }
        else if (fightNb == 5)
        {
            ChoseCharacterManager.instance.characterChoosed(4);
            ChoseCharacterManager.instance.enemyChoosed(2);
        }
        else if (fightNb == 6)
        {
            ChoseCharacterManager.instance.characterChoosed(5);
            ChoseCharacterManager.instance.enemyChoosed(3);
        }
        else if (fightNb == 7)
        {
            ChoseCharacterManager.instance.characterChoosed(1);
            ChoseCharacterManager.instance.enemyChoosed(7);
            bossBattle = true;
        }
        else if (fightNb == 8)
        {
            ChoseCharacterManager.instance.characterChoosed(4);
            ChoseCharacterManager.instance.enemyChoosed(8);
            bossBattle = true;
        }
        else if (fightNb == 9)
        {
            ChoseCharacterManager.instance.characterChoosed(1);
            ChoseCharacterManager.instance.enemyChoosed(9);
            bossBattle = true;
        }

        ChoseCharacterManager.instance.startGame();
    }

    public void backToMainMenu()
    {
        bossBattle = false;
        story = false;

        SceneManager.LoadScene("MainMenu");
    }
}
