using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameStates State;

    public GameObject canvas;

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
            case GameStates.Start:
                HandleStart();
                break;
            case GameStates.Dialogue:
                HandleDialogue();
                break;
            case GameStates.EndOfRound:
                HandleEndOfRound();
                break;
            case GameStates.Fight:
                HandleFight();
                break;
            case GameStates.Lose:
                break;
            case GameStates.Win:
                break;
        }

        StateChanged?.Invoke(newState);
    }

    private void HandleFight()
    {

    }

    private void HandleDialogue()
    {

    }

    private void HandleEndOfRound()
    {
        canvas.GetComponent<OffFinghtManager>().roundWon();
    }

    private void HandleStart()
    {
        //canvas.GetComponent<OffFinghtManager>().startOfFight();
    }
}

public enum GameStates
{
    Start,
    Dialogue,
    EndOfRound,
    Fight,
    Lose,
    Win
}