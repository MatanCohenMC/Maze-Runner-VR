using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private MazeGenerator m_MazeGenerator;
    [SerializeField] private Transform m_Player;
    [SerializeField] private Transform m_StarterRoom;
    private List<GameLevel> m_GameLevels;

    public GameLevel currentGameLevel { get; private set; }

    public void Awake()
    {
        m_GameLevels = new List<GameLevel>
        {
            new("Easy", 5, 5),
            new("Medium", 7, 7),
            new("Hard", 10, 10)
        };
    }

    public void SetGameLevel(string i_Name)
    {
        foreach (GameLevel gameLevel in m_GameLevels)
        {
            if (i_Name == gameLevel.Name)
            {
                currentGameLevel = gameLevel;
                mazePreparation();
                GameManager.Instance.StartGame();
                return;
            }
        }

        Debug.Log("No matching game level found for name: " + i_Name);
    }

    // Not in use right now
    public void SetCustomGameLevel(string i_Name, int i_Rows, int i_Cols)
    {
        bool isProperLevel = true;
        bool isNewLevel = true;

        foreach (GameLevel gameLevel in m_GameLevels)
        {
            if (i_Name == gameLevel.Name)
            {
                if (i_Rows != gameLevel.Rows || i_Cols != gameLevel.Cols)
                {
                    Debug.Log("Entered wrong level");
                    isProperLevel = false;
                    break;
                }

                isNewLevel = false;
            }
        }

        if (isProperLevel)
        {
            if (isNewLevel)
            {
                m_GameLevels.Add(new GameLevel(i_Name, i_Rows, i_Cols));
            }

            foreach (GameLevel gameLevel in m_GameLevels)
            {
                if (i_Name == gameLevel.Name)
                {
                    currentGameLevel = gameLevel;
                }
            }

            mazePreparation();
            GameManager.Instance.StartGame();
        }
    }

    private void mazePreparation()
    {
        // Generate the maze
        m_MazeGenerator.GenerateMazeInstant(currentGameLevel ,currentGameLevel.Rows, currentGameLevel.Cols);

        // Move player to the start of the maze
        movePlayerToStartNode();
    }

    private void movePlayerToStartNode()
    {
        Vector3 offset = new Vector3(0f, 0.5f, 0f);

        // m_Player.position = Vector3.zero;
        m_Player.position = m_MazeGenerator.StartNode.transform.position + offset;
    }

    public void EndTriggerEntered()
    {
        // Move player to the starter room
        movePlayerToStarterRoom();

        // Delete the maze
        m_MazeGenerator.DeleteMaze();
    }

    private void movePlayerToStarterRoom()
    {
        m_Player.position = Vector3.zero;
        m_Player.position = m_StarterRoom.transform.position;
    }
}
