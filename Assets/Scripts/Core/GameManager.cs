using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SystemSingleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        InitSystems();
    }

    private void InitSystems()
    {
        //Init your subsystem here!
        FXManager.Init();
    }
}
