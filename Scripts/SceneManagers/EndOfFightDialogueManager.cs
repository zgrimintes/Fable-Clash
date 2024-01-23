using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

        iconDialogueP.GetComponent<Image>().sprite = player.GetComponent<CharacterManager>().fighterManager.characterIcon;
        iconDialogueE.GetComponent<Image>().sprite = enemy.GetComponent<CharacterManager>().fighterManager.characterIcon;

        if (i == 0)
        {
            textDialogueP.text = player.GetComponent<CharacterManager>().fighterManager.Dialogues[1];
            textDialogueE.text = enemy.GetComponent<CharacterManager>().fighterManager.Dialogues[0];
        }
        else
        {
            textDialogueP.text = player.GetComponent<CharacterManager>().fighterManager.Dialogues[0];
            textDialogueE.text = enemy.GetComponent<CharacterManager>().fighterManager.Dialogues[1];
        }
    }

    public void reButtonClick()
    {
        if (firstD == 0) resetFight();
        else continueButton();
    }

    public void resetFight()
    {
        StoryTellingManager.nextStoryInstance -= 2; //Go back to the story instance of the start of the fight
        SceneManager.LoadScene("StoryTelling");
        StoryTellingManager.Instance.setFight(StoryTellingManager.currentFight);
    }

    public void continueButton()
    {
        StoryTellingManager.fightsWon[StoryTellingManager.currentFight] = true; //Set the current fight won
        storyCanvas.SetActive(true);
        SceneManager.LoadScene("StoryTelling");
        StoryTellingManager.Instance.nextStory(18);
    }
}
