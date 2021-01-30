using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SystemSingleton<GameManager>
{
    private GameObject m_player;
    private GameObject m_enemies;

    private GameObject uiPrefab;
    private GameObject mainMenuPanel;
    private GameObject gameplayPanel;

    public GameObject enemyPrefab;

    [SerializeField] private bool m_debug;

    public GameObject GetPlayer()
    {
        return m_player;
    }

    protected override void Awake()
    {
        base.Awake();
        if (m_debug)
        {
            InitSystems();

            SpawnPlayer();
        }
        else
        {
            uiPrefab = GameObject.Find("UIPrefab"); //Finds the UI prefab
            mainMenuPanel = GameObject.Find("MainMenuPanel");
            gameplayPanel = GameObject.Find("GameplayPanel");
            gameplayPanel.SetActive(false);
        }
    }

    private void InitSystems()
    {
        //Init your subsystem here!
        FXManager.Init();
        CameraManager.Init();
    }

    private void SpawnPlayer()
    {
        if (!m_player)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("player");
            m_player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    private void SpawnEnemies()
    {
        if(!m_enemies)
        {
            //GameObject enemyPrefab = Resources.Load<GameObject>("Gargoyle");
            m_enemies = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    //Function to be called to start a new game
    //Currently, OnClick calls this directly.  Bad form?
    public void StartGameplay()
    {
        mainMenuPanel.SetActive(false);  //Hides the main menu
        gameplayPanel.SetActive(true);  //Unhides the gameplay UI

        //Change to gameplay music

        InitSystems();
        SpawnPlayer(); //Spawns the player

        //Spawn enemies
        SpawnEnemies();
    }

    //------- Helpers -------
    public Vector3 GetMouseWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        return pos;

    }
}
