using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
        if (dialogues == 2) return;

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
        }
        else
        {
            dP.SetActive(false);
            textDialogueP.enabled = false;
            iconDialogueP.SetActive(false);

            dE.SetActive(true);
            textDialogueE.enabled = true;
            iconDialogueE.SetActive(true);
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
            rcButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
            textDialogueP.text = player.GetComponent<CharacterManager>().fighterManager.Dialogues[1];
            textDialogueE.text = enemy.GetComponent<CharacterManager>().fighterManager.Dialogues[0];
        }
        else
        {
            rcButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
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
        ChoseCharacterManager.instance.characterChoosed(1);
        ChoseCharacterManager.instance.enemyChoosed(6);
        ChoseCharacterManager.instance.startGame();
    }

    public void continueButton()
    {
        storyCanvas.SetActive(true);
        StoryTellingManager.Instance.nextStory(19);
    }
}
