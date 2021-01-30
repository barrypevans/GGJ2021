using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFuctions : MonoBehaviour
{
    public void StartGameplay()
    {
        GameManager.Get().StartGameplay();
    }

    public void ExitApplication()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
