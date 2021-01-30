using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SystemSingleton<GameManager>
{
    private GameObject m_player;

    private GameObject uiPrefab;
    private GameObject mainMenuPanel;
    private GameObject gameplayPanel;

    public GameObject GetPlayer()
    {
        return m_player;
    }

    protected override void Awake()
    {
        base.Awake();
        //InitSystems();

        //SpawnPlayer();
        uiPrefab = GameObject.Find("UIPrefab"); //Finds the UI prefab
        mainMenuPanel = GameObject.Find("MainMenuPanel");
        gameplayPanel = GameObject.Find("GameplayPanel");
        gameplayPanel.SetActive(false);
    }

    private void InitSystems()
    {
        //Init your subsystem here!
        FXManager.Init();
        CameraManager.Init();
    }

    private void SpawnPlayer()
    {
        if(!m_player)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("player");
            m_player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    //Function to be called to start a new game
    //Currently, OnClick calls this directly.  Bad form?
    public void StartGameplay()
    {
        mainMenuPanel.SetActive(false);
        gameplayPanel.SetActive(true);

        //Change to gameplay music

        InitSystems();
        SpawnPlayer();
    }
}
