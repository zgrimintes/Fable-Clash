using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameStates State;

    public static event Action<GameStates> StateChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void updateGameState(GameStates newState)
    {
        State = newState;

        switch (State)
        {
            case GameStates.Dialogue:
                HandleDialogue();
                break;
            case GameStates.EndOfRound:
                HandleEndOfRound();
                break;
            case GameStates.Fight:
                break;
            case GameStates.Lose:
                break;
            case GameStates.Win:
                break;
        }

        StateChanged?.Invoke(newState);
    }

    private void HandleDialogue()
    {

    }

    private void HandleEndOfRound()
    {
        GameObject enemy, player;
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        //if (round_won == 2) _win_condition_checkd

        SceneManager.LoadScene("SampleScene");
    }
}

public enum GameStates
{
    Dialogue,
    EndOfRound,
    Fight,
    Lose,
    Win
}