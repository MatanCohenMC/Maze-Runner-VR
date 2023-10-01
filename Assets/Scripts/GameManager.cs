using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public enum eGameState
{
    Idle,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private MazeManager m_MazeManager;
    public static GameManager Instance { get; private set; }
    public eGameState CurrentGameState { get; private set; }
    public string PlayerName { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Set the initial game state
        setGameStateToIdle();
    }

    private void setGameStateToIdle()
    {
        CurrentGameState = eGameState.Idle;
        Debug.Log("Game state: Idle");
    }

    private void setGameStateToPlaying()
    {
        CurrentGameState = eGameState.Playing;
        Debug.Log("Game state: Playing");
    }

    private void setGameStateToGameOver()
    {
        CurrentGameState = eGameState.GameOver;
        Debug.Log("Game state: GameOver");
    }

    public void StartGame()
    {
        if (CurrentGameState == eGameState.Idle)
        {
            setGameStateToPlaying();
            // 3 seconds count down
            // timer appear in the sky
        }
    }

    public void EndGame()
    {
        m_MazeManager.EndTriggerEntered();
    }

    public void SetPlayerName(string name)
    {
        if (name != string.Empty)
        {
            PlayerName = name;
            Debug.Log($"Player name set to: {PlayerName}");
        }
    }
}
