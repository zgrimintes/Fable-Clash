using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfFightDialogueManager : MonoBehaviour
{
    public GameObject player, enemy;
    public TextMeshProUGUI textDialogueP, textDialogueE;
    public GameObject iconDialogueP, iconDialogueE, dP, dE;
    public GameObject rcButton;
    public GameObject storyCanvas;

    int dialogues = 1;
    int firstD;

    public static int benchCh = 1;

    public void nextDialogue()
    {
        if (dialogues == 2)
        {
            reButtonClick();
            return;
        }

        rcButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next"; //Reset the text
        dialogues++;
        if (firstD == 0)
        {
            //First hide the enemy dialogue elements
            dE.SetActive(false);
            textDialogueE.enabled = false;
            iconDialogueE.SetActive(false);

            //Then show the player's
            dP.SetActive(true);
            textDialogueP.enabled = true;
            iconDialogueP.SetActive(true);

            rcButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
        }
        else
        {
            dP.SetActive(false);
            textDialogueP.enabled = false;
            iconDialogueP.SetActive(false);

            dE.SetActive(true);
            textDialogueE.enabled = true;
            iconDialogueE.SetActive(true);

            rcButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        }
    }

    public void startOfDialogue(int i) //0 - has lost; 1 - has won
    {
        firstD = i; //Set the starting dialogue
        dialogues = 1;
        if (i == 3 && benchCh < 3) benchCh = 4;

        iconDialogueP.GetComponent<Image>().sprite = player.GetComponent<CharacterManager>().fighterManager.characterIcon;
        iconDialogueE.GetComponent<Image>().sprite = enemy.GetComponent<CharacterManager>().fighterManager.characterIcon;

        if (i == 0)
        {
            if (StoryTellingManager.bossBattle)
            {
                if (StoryTellingManager.currentFight == 7) textDialogueP.text = "Don’t give up now! Everything is on the line!";
                else if (StoryTellingManager.currentFight == 8) textDialogueP.text = "Are we really going to let him one-up us?!";
                else if (StoryTellingManager.currentFight == 9) textDialogueP.text = "This..isn’t a fitting finale...";
            }
            else
                textDialogueP.text = player.GetComponent<CharacterManager>().fighterManager.Dialogues[1];

            textDialogueE.text = enemy.GetComponent<CharacterManager>().fighterManager.Dialogues[0];
        }
        else if (i == 1)
        {
            if (StoryTellingManager.bossBattle)
            {
                if (StoryTellingManager.currentFight == 7) textDialogueP.text = "We did our part!";
                else if (StoryTellingManager.currentFight == 8) textDialogueP.text = "I bet we finished this faster than those kids";
                else if (StoryTellingManager.currentFight == 9) textDialogueP.text = "We did it! We’re Saved!";
            }
            else
                textDialogueP.text = player.GetComponent<CharacterManager>().fighterManager.Dialogues[0];

            textDialogueE.text = enemy.GetComponent<CharacterManager>().fighterManager.Dialogues[1];
        }
        else if (i == 2 || i == 3)
        {
            dialogues = 2;
            rcButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
            textDialogueP.text = player.GetComponent<CharacterManager>().fighterManager.Dialogues[2];
        }
        else
        {
            dialogues = 2;
            rcButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
            textDialogueP.text = player.GetComponent<CharacterManager>().fighterManager.Dialogues[3];
        }
    }

    public void reButtonClick()
    {
        if (firstD == 0) resetFight();
        else if (firstD == 1) continueButton();
        else nextBenchCh();
    }

    public void nextBenchCh()
    {
        ChoseCharacterManager.instance.characterChoosed((StoryTellingManager.currentFight != 9) ? ++benchCh : nextOneFinalBattle());
        OffFinghtManager.Instance.startOfFight(false);
        OffFinghtManager.Instance.startFight();
    }

    public int nextOneFinalBattle()
    {
        switch (benchCh)
        {
            case 1:
                benchCh = 6;
                OffFinghtManager.bench = 5;
                break;
            case 6:
                benchCh = 2;
                break;
            case 2:
                benchCh = 4;
                break;
            case 4:
                benchCh = 3;
                break;
            case 3:
                benchCh = 5;
                break;
        }

        return benchCh;
    }

    public void resetFight()
    {
        SceneManager.LoadScene("StoryTelling");
        StoryTellingManager.Instance.setFight(StoryTellingManager.currentFight);
    }

    public void continueButton()
    {
        StoryTellingManager.fightsWon[StoryTellingManager.currentFight] = true; //Set the current fight won
        storyCanvas.SetActive(true);
        SceneManager.LoadScene("StoryTelling");
    }
}
