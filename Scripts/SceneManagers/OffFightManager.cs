using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OffFinghtManager : MonoBehaviour
{
    public static OffFinghtManager Instance;

    TextMeshProUGUI countdown, scoreIndicator;
    GameObject enemy, player;

    public GameObject button;
    public GameObject roundsWonTextP, roundsWonTextE;

    public bool game = false;

    private void Awake()
    {
        initializeCanvas();
        Instance = this;

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

    public void minusOneCountdown(int n)
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
            case 0:
                startFight();
                break;
        }
    }

    public async void roundWon()
    {
        enemy.GetComponent<EnemyController>().canAttack = false;
        player.GetComponent<PlayerManager>().canMove = false;

        gameObject.SetActive(true);
        scoreIndicator.enabled = true;

        int winsPlayer = player.GetComponent<CharacterManager>().fighterManager.roundsWon,
            winsEnemy = enemy.GetComponent<CharacterManager>().fighterManager.roundsWon;

        if (winsEnemy == 2)
        {
            Time.timeScale = 0;
            game = false;
            button.SetActive(true);
            scoreIndicator.text = enemy.GetComponent<CharacterManager>().fighterManager.characterName + " has won!";
            roundsWonTextE.GetComponent<TextMeshProUGUI>().text = "2";
            //startOfFight(); //reset the rounds won by each character
            return;
        }
        else if (winsPlayer == 2)
        {
            Time.timeScale = 0;
            game = false;
            button.SetActive(true);
            scoreIndicator.text = player.GetComponent<CharacterManager>().fighterManager.characterName + " has won!";
            roundsWonTextP.GetComponent<TextMeshProUGUI>().text = "2";
            //startOfFight(); //reset the rounds won by each character
            return;
        }
        else
        {
            scoreIndicator.text = winsPlayer + " - " + winsEnemy;
        }

        await Task.Delay(1500);

        rematch();
    }

    public void rematch()
    {
        resetEnvironment();

        scoreIndicator.enabled = false;
        countdown.text = "3";
        countdown.enabled = true;
        countdown.GetComponent<Animator>().Play("Countdown");

        //Reset the stats, position and effects of both characters
        enemy.GetComponent<CharacterManager>().startFight();
        enemy.transform.position = new Vector2(10.58f, -.5f);
        enemy.transform.localScale = new Vector2(-enemy.transform.localScale.x, enemy.transform.localScale.y);
        enemy.GetComponent<CharacterManager>().rb.velocity = Vector3.zero;

        player.GetComponent<CharacterManager>().startFight();
        player.transform.position = new Vector2(-10.58f, -.5f);
        player.transform.localScale = new Vector2(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        player.GetComponent<CharacterManager>().horizontalS = 1;
        player.GetComponent<CharacterManager>().rb.velocity = Vector3.zero;

        GameManager.Instance.updateGameState(GameStates.Fight);
    }

    public void resetEnvironment()
    {
        GameObject darkMist = GameObject.Find("DarkMist(Clone)");
        GameObject boomerang = GameObject.Find("Boomerang(Clone)");

        if (darkMist != null) Destroy(darkMist);
        if (boomerang != null) Destroy(boomerang);
    }

    public void startFight()
    {
        Time.timeScale = 1;
        enemy.GetComponent<EnemyController>().waitState.StartFight();
        player.GetComponent<PlayerManager>().canMove = true;
        gameObject.SetActive(false);
    }

    public void startOfFight()
    {
        Time.timeScale = 1; //Reset the flowing of time
        game = true;
        enemy.GetComponent<CharacterManager>().fighterManager.roundsWon = 0;
        player.GetComponent<CharacterManager>().fighterManager.roundsWon = 0;
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
