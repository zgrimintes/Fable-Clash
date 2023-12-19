using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameStates State;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        updateGameState(GameStates.Dialogue);
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
                break;
            case GameStates.Fight:
                break;
            case GameStates.Lose:
                break;
            case GameStates.Win:
                break;
        }
    }

    private void HandleDialogue()
    {

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