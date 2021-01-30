using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SystemSingleton<GameManager>
{
    private GameObject m_player;

    public GameObject GetPlayer()
    {
        return m_player;
    }

    protected override void Awake()
    {
        base.Awake();
        InitSystems();

        SpawnPlayer();
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

    //------- Helpers -------
    public Vector3 GetMouseWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        return pos;
    }
}
