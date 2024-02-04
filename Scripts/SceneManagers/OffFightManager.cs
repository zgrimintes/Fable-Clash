using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OffFinghtManager : MonoBehaviour
{
    public static OffFinghtManager Instance;

    TextMeshProUGUI countdown, scoreIndicator;
    GameObject enemy, player;

    public GameObject button;
    public GameObject roundsWonTextP, roundsWonTextE;
    public GameObject dialogueCanvas, dialogueP, dialogueE;

    public bool game = false;
    public static int bench = 3;

    private void Awake()
    {
        Time.timeScale = 1;
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
            case 4:
                countdown.text = "3";
                break;
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
        int winsPlayer = player.GetComponent<CharacterManager>().fighterManager.roundsWon,
            winsEnemy = enemy.GetComponent<CharacterManager>().fighterManager.roundsWon;

        enemy.GetComponent<EnemyController>().canAttack = false;
        player.GetComponent<PlayerManager>().canMove = false;

        gameObject.SetActive(true);
        if (!StoryTellingManager.bossBattle || winsEnemy == 2 || winsPlayer == 2) scoreIndicator.enabled = true;


        if (winsEnemy == 2)
        {
            game = false;
            scoreIndicator.text = enemy.GetComponent<CharacterManager>().fighterManager.characterName + " has won!";
            roundsWonTextE.GetComponent<TextMeshProUGUI>().text = "2";
            Time.timeScale = 0;

            if (StoryTellingManager.story)
            {
                await Task.Delay(2000);
                fadeText(0);
                return;
            }

            button.SetActive(true);
            //startOfFight(); //reset the rounds won by each character
            return;
        }
        else if (winsPlayer == 2)
        {
            game = false;
            if (!StoryTellingManager.bossBattle) scoreIndicator.text = player.GetComponent<CharacterManager>().fighterManager.characterName + " has won!";
            else
            {
                scoreIndicator.text = "The Realm of Men has won!";
            }

            roundsWonTextP.GetComponent<TextMeshProUGUI>().text = "2";
            Time.timeScale = 0;

            if (StoryTellingManager.story)
            {
                await Task.Delay(1200);
                fadeText(1);
                return;
            }


            button.SetActive(true);
            //startOfFight(); //reset the rounds won by each character
            return;
        }
        else
        {
            scoreIndicator.text = winsPlayer + " - " + winsEnemy;
        }

        player.GetComponent<AttackManager>().loopAnimation(0);
        enemy.GetComponent<AttackManager>().loopAnimation(0);
        player.GetComponent<CharacterManager>().isDangerous = false;
        enemy.GetComponent<CharacterManager>().isDangerous = false;

        Time.timeScale = 0;
        if (!StoryTellingManager.bossBattle) await Task.Delay(1500);
        Time.timeScale = 1;
        rematch();
    }

    public void fadeText(int i)
    {
        scoreIndicator.text = "3";
        scoreIndicator.enabled = false;
        dialogueCanvas.SetActive(true);
        if (i == 0)
        {
            dialogueE.SetActive(true);
            dialogueP.SetActive(false);
        }
        else if (i == 1)
        {
            dialogueP.SetActive(true);
            dialogueE.SetActive(false);
        }
        else
        {
            dialogueP.SetActive(true);
            dialogueE.SetActive(false);
        }

        dialogueCanvas.GetComponent<EndOfFightDialogueManager>().startOfDialogue(i);
    }

    public void rematch()
    {
        resetEnvironment();

        if (StoryTellingManager.bossBattle)
        {
            nextBench();
            return;
        }

        startCountdown();

        //Reset the stats, position and effects of both characters
        resetTraits(enemy);
        enemy.transform.position = new Vector2(10.58f, -.5f);
        enemy.transform.localScale = new Vector2(-enemy.transform.localScale.x, enemy.transform.localScale.y);

        resetTraits(player);
        player.transform.position = new Vector2(-10.58f, -.5f);
        player.transform.localScale = new Vector2(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        player.GetComponent<CharacterManager>().horizontalS = 1;

        GameManager.Instance.updateGameState(GameStates.Fight);
    }

    public void resetTraits(GameObject ch)
    {
        ch.GetComponent<CharacterManager>().startFight();
        ch.GetComponent<CharacterManager>().stopAllAnims();

        ch.GetComponent<CharacterManager>().rb.velocity = Vector3.zero;
        ch.GetComponent<SpecialAttacksManager>().StopAllCoroutines(); //Stop all corutines that may happen 
        ch.GetComponent<MagicAbilitiesManager>().StopAllCoroutines();
    }

    public void nextBench()
    {
        bench--;
        if (bench <= 0)
        {
            enemy.GetComponent<CharacterManager>().fighterManager.roundsWon = 2;
            roundWon();
            return;
        }

        if (player.GetComponent<CharacterManager>().fighterManager.roundsWon == 1)
        {
            player.GetComponent<CharacterManager>().fighterManager.roundsWon = 2;
            roundWon();
            return;
        }

        if (enemy.GetComponent<CharacterManager>().fighterManager.roundsWon >= 1) enemy.GetComponent<CharacterManager>().fighterManager.roundsWon--;
        fadeText(StoryTellingManager.currentFight - 5);
        Time.timeScale = 0;
    }

    public void startCountdown()
    {
        scoreIndicator.enabled = false;
        countdown.text = "3";
        countdown.enabled = true;
        countdown.GetComponent<Animator>().Play("Countdown");
    }

    public void resetEnvironment()
    {
        GameObject darkMist = GameObject.Find("DarkMist(Clone)");
        GameObject boomerang = GameObject.Find("Boomerang(Clone)");
        GameObject PProjectile = GameObject.Find("PProjectile(Clone)");
        GameObject EProjectile = GameObject.Find("EProjectile(Clone)");
        GameObject arrowC = GameObject.Find("arrow-color(Clone)");
        GameObject windShield = GameObject.Find("WindShield(Clone)");

        if (darkMist != null) Destroy(darkMist);
        if (boomerang != null) Destroy(boomerang);
        if (PProjectile != null) Destroy(PProjectile);
        if (EProjectile != null) Destroy(EProjectile);
        if (arrowC != null) Destroy(arrowC);
        if (windShield != null) Destroy(windShield);
    }

    public void startFight()
    {
        Time.timeScale = 1;
        enemy.GetComponent<EnemyController>().waitState.StartFight();
        player.GetComponent<PlayerManager>().canMove = true;
        gameObject.SetActive(false);
    }

    public void startOfFight(bool allReset)
    {
        if (StoryTellingManager.story)
        {
            dialogueCanvas.SetActive(false);
        }

        resetEnvironment();

        Time.timeScale = 1; //Reset the flowing of time
        game = true;
        if (allReset)
        {
            EndOfFightDialogueManager.benchCh = 1;
            bench = 3;
            enemy.GetComponent<CharacterManager>().fighterManager.roundsWon = 0;
            enemy.GetComponent<CharacterManager>().fighterManager.startOfFight();
        }

        enemy.transform.position = new Vector2(10.58f, -.5f);
        enemy.GetComponent<CharacterManager>().hasLost = false;
        roundsWonTextE.GetComponent<TextMeshProUGUI>().text = "0";

        player.GetComponent<CharacterManager>().fighterManager.roundsWon = 0;
        player.GetComponent<CharacterManager>().fighterManager.startOfFight();
        player.transform.position = new Vector2(-10.58f, -.5f);
        player.GetComponent<CharacterManager>().hasLost = false;
        roundsWonTextP.GetComponent<TextMeshProUGUI>().text = "0";
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
