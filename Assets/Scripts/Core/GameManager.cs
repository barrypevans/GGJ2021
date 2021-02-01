using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SystemSingleton<GameManager>
{
    private GameObject m_player;
    private GameObject m_enemies;

    public GameObject enemyPrefab;
    public GameObject m_particlePrefab;

    public bool m_gameStarted = false;
    public bool m_gameOver = false;

    public Transform m_playerSpawn1;
    public Transform m_playerSpawn2;

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
        m_particlePrefab = Resources.Load<GameObject>("fx/generic-particles");
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
            m_player = Instantiate(playerPrefab, m_playerSpawn1.position, Quaternion.identity);
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
        UiManager.Get().PlayStory(0);
        FXManager.Get().SetMusic(null);
    }

    public void StoryCompleted(int story)
    {
        switch (story)
        {
            case 0:
                {
                    FXManager.Get().SetMusic("music/Gameplay Music Wave 1");
                    EnemyManager.Get().SetLocations();
                    EnemyManager.Get().StartWave();
                    m_gameStarted = true;
                    break;
                }
            case 1:
                {
                    FXManager.Get().SetMusic("music/Gameplay Music Wave 2 V.2");
                    EnemyManager.Get().StartWave(story);
                    break;
                }
            case 2:
                {
                    FXManager.Get().SetMusic("music/Gameplay Music Wave 3 V.2");
                    EnemyManager.Get().StartWave(story);
                    break;
                }
            case 3:
                {
                    FXManager.Get().SetMusic("music/Play Again Theme V.1");
                    Win();
                    break;
                }
        }
    }

    public void WaveCompleted(int wave)
    {
        UiManager.Get().PlayStory(wave+1);
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
        //Debug.Log("Game Over");
        FXManager.Get().SetMusic(null);
        FXManager.Get().PlaySFX("sfx/game over", 0F, 0.5F);
        m_player.SetActive(false);
        m_player.GetComponent<Player>().m_gun.SetActive(false);

        StartCoroutine(Co_LoseCard());
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
            Transform[] enemyTargetLocations = EnemyManager.Get().GetTargetLocations();
            float distFromPlayerSpawn1 = 0F;
            float distFromPlayerSpawn2 = 0F;
            for (int i = 0; i < enemyTargetLocations.Length; i++)
            {
                distFromPlayerSpawn1 += Vector3.Distance(m_playerSpawn1.position, enemyTargetLocations[i].position);
                distFromPlayerSpawn2 += distFromPlayerSpawn2 + Vector3.Distance(m_playerSpawn2.position, enemyTargetLocations[i].position);
            }
            if (distFromPlayerSpawn1 > distFromPlayerSpawn2)
                m_player.transform.position = m_playerSpawn1.position;
            else
                m_player.transform.position = m_playerSpawn2.position;
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
        UiManager.Get().ShowWinPanel(true);
        yield return new WaitForSeconds(21);
        UiManager.Get().ShowWinPanel(false);
        SceneManager.LoadScene(0);
    }

    IEnumerator Co_LoseCard()
    {
        // pop up win card here
        UiManager.Get().ShowLosePanel(true);
        yield return new WaitForSeconds(6);
        UiManager.Get().ShowLosePanel(false);
        m_gameOver = true;
        m_gameStarted = false;
        SceneManager.LoadScene(0);
    }

    //------- Helpers -------
    public Vector3 GetMouseWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        return pos;
    }

    public void SpawnParticles(Vector3 location, Color color)
    {
        var part = Instantiate(m_particlePrefab, location, Quaternion.identity);
        var main = part.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        Destroy(part, 3.0f);
    }
}
