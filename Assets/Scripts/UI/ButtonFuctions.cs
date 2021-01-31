using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void PauseResumeGame(Toggle t)
    {
        Debug.Log(t);
        if (t.isOn)
        {
            FXManager.Get().PlaySFX("Unpause");
            Time.timeScale = 1;
        }
        else
        {
            FXManager.Get().PlaySFX("Pause");
            Time.timeScale = 0;
        }
    }

    /*public void ResumeGame()
    {
        Time.timeScale = 1;
    }*/
}
