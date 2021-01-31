using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SystemSingleton<GameManager>
{
    private GameObject m_player;
    private GameObject m_enemies;

    public GameObject enemyPrefab;

    public bool m_debugMusicOff;

    public GameObject GetPlayer()
    {
        return m_player;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitSystems();
    }

    private void InitSystems()
    {
        //Init your subsystem here!
        FXManager.Init();
        CameraManager.Init();
        EnemyManager.Init();
        UiManager.Init();
    }

    private void SpawnPlayer()
    {
        if (!m_player)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("player");
            m_player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    //Function to be called to start a new game
    //Currently, OnClick calls this directly.  Bad form?
    public void StartGameplay()
    {
        //Change to gameplay music
        SpawnPlayer(); //Spawns the player

        UiManager.Get().ShowGameplayPanel();
        //FXManager.Get().PlaySFX("Pause");
        FXManager.Get().SetMusic("Goth_V.1");
        EnemyManager.Get().SetLocations();
        EnemyManager.Get().StartWave();
    }

    //------- Helpers -------
    public Vector3 GetMouseWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        return pos;
    }
}
