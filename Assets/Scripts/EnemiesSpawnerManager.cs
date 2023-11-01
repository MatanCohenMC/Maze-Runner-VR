using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesSpawnerManager : MonoBehaviour
{
    [SerializeField] private int m_EnemyDuplicationCount = 3;
    [SerializeField] private List<GameObject> m_EasyEnemiesToSpawn;
    [SerializeField] private List<GameObject> m_AdvancedEnemiesToSpawn;
    [SerializeField] private int m_MaxEnemyCount = 2;
    [SerializeField] private float m_SecondsToWaitBetweenSpawningEnemies = 5;
    private int m_CurrEnemyCount;
    private float m_NextSpawnTime;
    private List<GameObject> m_EasyEnemiesToSpawnStorage = new List<GameObject>();
    private List<GameObject> m_AdvancedEnemiesToSpawnStorage = new List<GameObject>();
    private MazeManager m_MazeManager;
    private Transform m_PointToSpawnEnemies;

    public List<GameObject> EasyEnemiesToSpawnStorage { get { return m_EasyEnemiesToSpawnStorage; } }
    public List<GameObject> AdvancedEnemiesToSpawnStorage { get { return m_AdvancedEnemiesToSpawnStorage; } }

    void Start()
    {
        m_MazeManager = GameObject.Find("Maze Manager").GetComponent<MazeManager>();
        GameManager.OnPlayModeStart += UpdateBeginTimeToSpawnEnemies;
        setStorageOfEasyEnemiesToSpawn();
        setStorageOfAdvancedEnemiesToSpawn();
    }

    public void UpdateBeginTimeToSpawnEnemies()
    {
        m_NextSpawnTime = Time.time + m_SecondsToWaitBetweenSpawningEnemies;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState == eGameState.Playing)
        {
            // Update active enemies's position on maze
            switch (m_MazeManager.CurrentGameLevel.Name)
            {
                case "Medium":
                    UpdateEnemyAgentDestinationToMainCamera(m_EasyEnemiesToSpawnStorage);
                    if (m_CurrEnemyCount < m_MaxEnemyCount && Time.time > m_NextSpawnTime)
                    {
                        // Spawn more enemies
                        SpawnEnemyOnStartMaze(m_EasyEnemiesToSpawnStorage);
                    }
                    break;

                case "Hard":
                    UpdateEnemyAgentDestinationToMainCamera(m_AdvancedEnemiesToSpawnStorage);
                    if (m_CurrEnemyCount < m_MaxEnemyCount && Time.time > m_NextSpawnTime)
                    {
                        // Spawn more enemies
                        SpawnEnemyOnStartMaze(m_AdvancedEnemiesToSpawnStorage);
                    }
                    break;

                default:
                    // Debug.Log("This level doesn't include enemy to spawn");
                    break;
            }                  
        }                 
    }

    public void PrepareToSpawnEnemies(Transform i_PointToSpawnEnemies)
    {
        m_PointToSpawnEnemies = i_PointToSpawnEnemies;
    }

    public void SpawnEnemyOnStartMaze(List<GameObject> i_EnemyStorage)
    {
        //bool didUnactiveEnemyFind = false;

        // Get a random integer between 0 (inclusive) and i_EnemyStorage.Count (exclusive).
        int randomIndex = Random.Range(0, i_EnemyStorage.Count);
        if (!i_EnemyStorage[randomIndex].activeSelf) // if enemy is not active
        {
            //didUnactiveEnemyFind = true;
            if (i_EnemyStorage[randomIndex].GetComponent<Animator>().GetBool("isDead"))
            {
                // enemy was in the game already and died
                initEnemySettings(i_EnemyStorage[randomIndex]);
            }

            i_EnemyStorage[randomIndex].SetActive(true);
            m_CurrEnemyCount++;
            m_NextSpawnTime = Time.time + m_SecondsToWaitBetweenSpawningEnemies;
        }

        //while (!didUnactiveEnemyFind)
        //{
            
        //}       
    }

    public void MakeEnemyDead(GameObject enemy)
    {
        enemy.GetComponent<Animator>().SetBool("isDead", true);
        // stop enemy to run after the player
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        m_CurrEnemyCount--;
    }

    public void MakeEnemyUnactive()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void initEnemySettings(GameObject enemy)
    {
        enemy.transform.SetPositionAndRotation(m_PointToSpawnEnemies.position, m_PointToSpawnEnemies.rotation);
        // make enemy run after the player 
        transform.GetComponent<NavMeshAgent>().isStopped = false;
        enemy.GetComponent<HealthManager>().ResetHealth();
        enemy.GetComponent<Animator>().SetBool("isDead", false);
    }

    private void setStorageOfEnemiesToSpawn(List<GameObject> i_EnemyToSpawnList, List<GameObject> i_EnemyToSpawnListStorage, string i_NameOfStorage)
    {
        // Create a new empty GameObject
        GameObject StorageOfEnemiesToSpawn = new GameObject(i_NameOfStorage);

        // Find the GameObject with the name "EnemiesSpawner"
        GameObject enemiesSpawner = GameObject.Find("EnemiesSpawner");

        // Check if the GameObject was not found
        if (enemiesSpawner == null)
        {
            Debug.LogWarning("No GameObject with the name 'EnemiesSpawner' found.");
        }

        // Set the parent of the new GameObject to EnemiesSpawner
        StorageOfEnemiesToSpawn.transform.SetParent(enemiesSpawner.transform);

        foreach (GameObject enemy in i_EnemyToSpawnList)
        {
            for (int i = 0; i < m_EnemyDuplicationCount; i++)
            {
                GameObject duplicatedEnemy = Instantiate(enemy);
                duplicatedEnemy.SetActive(false);
                duplicatedEnemy.transform.SetParent(StorageOfEnemiesToSpawn.transform);
                i_EnemyToSpawnListStorage.Add(duplicatedEnemy);
            }
        }
    }

    public void UpdateEnemyAgentDestinationToMainCamera(List<GameObject> i_Enemies)
    {
        foreach (GameObject enemy in i_Enemies)
        {
            if (enemy.active)
            {
                enemy.GetComponent<NavMeshAgent>().destination = GameObject.Find("Main Camera").transform.position;
            }
        }   
    }

    private void setStorageOfEasyEnemiesToSpawn()
    {
        setStorageOfEnemiesToSpawn(m_EasyEnemiesToSpawn, m_EasyEnemiesToSpawnStorage, "StorageOfEasyEnemiesToSpawn");
    }

    private void setStorageOfAdvancedEnemiesToSpawn()
    {
        setStorageOfEnemiesToSpawn(m_AdvancedEnemiesToSpawn, m_AdvancedEnemiesToSpawnStorage, "StorageOfAdvancedEnemiesToSpawn");
    }
}
