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
    public static event System.Action OnPlayModeStart;
    [SerializeField] private MazeManager m_MazeManager;
    [SerializeField] private GameObject m_EnemiesAndObsticlesManager;
    [SerializeField] private Timer m_Timer;
    [SerializeField] private HealthManager m_HealthManager;
    [SerializeField] private GameOver m_GameOver;
    [SerializeField] private LeaderboardManager m_LeaderboardManager;
    private const string k_DefaultPlayerName = "Player";

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
        GamePreparation();
    }

    private void Start()
    {
        SetPlayerName(k_DefaultPlayerName);
    }

    public void GamePreparation()
    {
        setGameStateToIdle();
        m_Timer.ResetTimer();
        m_HealthManager.ResetHealth();
        m_HealthManager.OnDeath += lostGame;
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
            // Start the timer
            m_Timer.StartTimer();
        }

        // Trigger the action when play mode starts
        if (OnPlayModeStart != null)
        {
            OnPlayModeStart.Invoke();
        }
    }

    public void EndGame(eGameOver i_EndGameReason)
    {
        setGameStateToGameOver();
        m_MazeManager.ExitMaze();
        m_Timer.StopTimer();

        if(i_EndGameReason == eGameOver.Win)
        {
            m_LeaderboardManager.SetPlayerScore();
            m_LeaderboardManager.AddScoreToLeaderBoard(new Score(PlayerName, m_LeaderboardManager.GetPlayerScore()), m_MazeManager.CurrentGameLevel);
        }

        m_GameOver.DisplayGameOverMenu(i_EndGameReason);
    }

    private IEnumerator executeAfterDelay(float delay)
    {
        Debug.Log("Coroutine started. Waiting for " + delay + " seconds.");

        yield return new WaitForSeconds(delay);

        // Code to be executed after the delay
        Debug.Log("Coroutine resumed after " + delay + " seconds.");
    }

    private IEnumerator ExecuteAfterDelay(float delay)
    {
        Debug.Log("Coroutine started. Waiting for " + delay + " seconds.");

        yield return new WaitForSeconds(delay);

        // Code to be executed after the delay
        Debug.Log("Coroutine resumed after " + delay + " seconds.");
    }

    public void SetPlayerName(string name)
    {
        if (name != string.Empty)
        {
            PlayerName = name;
            Debug.Log($"Player name set to: {PlayerName}");
        }
    }

    private void lostGame()
    {
        EndGame(eGameOver.Lose);
    }

    public void QuitGame()
    {
        EndGame(eGameOver.Quit);
    }
}
