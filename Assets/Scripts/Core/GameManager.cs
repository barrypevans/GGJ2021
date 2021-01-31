using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SystemSingleton<GameManager>
{
    private GameObject m_player;
    private GameObject m_enemies;

    public GameObject enemyPrefab;

    public bool m_gameStarted = false;
    public bool m_gameOver = false;

    public Transform m_playerSpawn;
    public bool m_debugMusicOff;
    public float m_resetPlayerTimer = 10;
    public float m_resetGameTimer = 0;
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
            m_player = Instantiate(playerPrefab, m_playerSpawn.position, Quaternion.identity);
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
        FXManager.Get().SetMusic("music/Goth_V.1");
        EnemyManager.Get().SetLocations();
        EnemyManager.Get().StartWave();
        m_gameStarted = true;
    }

    public void WaveCompleted(int wave)
    {
        if (wave == 2)
        {
            Win();
            return;
        }
        EnemyManager.Get().StartWave(wave + 1);
    }

    public void ResetPlayer()
    {
        m_resetPlayerTimer = 0;
        m_player.SetActive(false);
        m_player.GetComponent<Player>().m_gun.SetActive(false);
        m_player.GetComponent<Player>().ResetGun();
    }

    public void GameOver()
    {
        //Game over Screen, music, etc
        Debug.Log("Game Over");
        m_player.SetActive(false);
        m_player.GetComponent<Player>().m_gun.SetActive(false);
        m_gameOver = true;
        m_gameStarted = false;
    }

    public void Update()
    {
        if(m_gameOver)
        {
            if (m_resetGameTimer > 2)
                SceneManager.LoadScene(0);
            m_resetGameTimer += Time.deltaTime;
        }

        //Respawn player logic
        if (m_gameStarted &&
            m_player.activeSelf == false &&
            m_resetPlayerTimer > 2)
        {
            m_player.transform.position = m_playerSpawn.position;
            m_player.SetActive(true);
            m_player.GetComponent<Player>().m_gun.SetActive(true);
        }
        m_resetPlayerTimer += Time.deltaTime;
    }

    public void Win()
    {
        m_player.SetActive(false);
        m_player.GetComponent<Player>().m_gun.SetActive(false);

        // pop up win card
        StartCoroutine(Co_WinCard());
    }

    IEnumerator Co_WinCard()
    {
        // pop up win card here
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

    //------- Helpers -------
    public Vector3 GetMouseWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        return pos;
    }

}
