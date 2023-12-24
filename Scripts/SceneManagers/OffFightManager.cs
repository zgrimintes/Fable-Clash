using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OffFinghtManager : MonoBehaviour
{
    TextMeshProUGUI countdown, scoreIndicator;
    GameObject enemy, player;

    public GameObject button;

    private void Awake()
    {
        initializeCanvas();

        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
    }

    private void initializeCanvas()
    {
        TextMeshProUGUI[] children = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < children.Length; i++)
        {
            switch (children[i].name)
            {
                case "CountdownText":
                    countdown = children[i];
                    countdown.text = "3";
                    break;
                case "ScoreIndicator":
                    scoreIndicator = children[i];
                    scoreIndicator.enabled = false;
                    break;
            }
        }
    }

    private void minusOneCountdown(int n)
    {
        switch (n)
        {
            case 3:
                countdown.text = "2";
                break;
            case 2:
                countdown.text = "1";
                break;
            case 1:
                countdown.text = "Fight!";
                break;
        }
    }

    public async void roundWon()
    {
        GameObject enemy, player;
        TextMeshProUGUI scoreIndicator;
        enemy = GameObject.Find("Enemy"); enemy.GetComponent<EnemyController>().canAttack = false;
        player = GameObject.Find("Player"); player.GetComponent<PlayerManager>().canMove = false;
        gameObject.SetActive(true);
        scoreIndicator = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        scoreIndicator.enabled = true;

        int winsPlayer = player.GetComponent<CharacterManager>().fighterManager.roundsWon,
            winsEnemy = enemy.GetComponent<CharacterManager>().fighterManager.roundsWon;

        if (winsEnemy == 2)
        {
            button.SetActive(true);
            scoreIndicator.text = enemy.GetComponent<CharacterManager>().fighterManager.name + " has won!";
            startOfFight(); //reset the rounds won by each character
            return;
        }
        else if (winsPlayer == 2)
        {
            button.SetActive(true);
            scoreIndicator.text = player.GetComponent<CharacterManager>().fighterManager.name + " has won!";
            startOfFight(); //reset the rounds won by each character
            return;
        }
        else
        {
            scoreIndicator.text = winsPlayer + " - " + winsEnemy;
        }

        await Task.Delay(1500);

        SceneManager.LoadScene("SampleScene");
    }

    public void startFight()
    {
        enemy.GetComponent<EnemyController>().waitState.StartFight();
        player.GetComponent<PlayerManager>().canMove = true;
        GetComponentInParent<Canvas>().gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void startOfFight()
    {
        enemy.GetComponent<CharacterManager>().fighterManager.roundsWon = 0;
        player.GetComponent<CharacterManager>().fighterManager.roundsWon = 0;
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
