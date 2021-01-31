using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFuctions : MonoBehaviour
{
    [SerializeField] private Transform m_ammoPanel;
    [SerializeField] private Transform m_healthPanel;

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
            FXManager.Get().PlaySFX("sfx/Unpause");
            Time.timeScale = 1;
        }
        else
        {
            FXManager.Get().PlaySFX("sfx/Pause");
            Time.timeScale = 0;
        }
    }

    public void SetAmmoLevel(int ammoLevel)
    {
        for(int i=0; i< 10; ++i)
        {
            Image image = m_ammoPanel.GetChild(i).GetComponent<Image>();
            image.color = 9-i >= ammoLevel ? new Color(1, 1, 1, .4f) :  Color.white ;
        }
    }

    public void SetHealthlevel(int healthLevel)
    {
        for (int i = 0; i < 3; ++i)
        {
            Image image = m_healthPanel.GetChild(i).GetComponent<Image>();
            image.color = i < healthLevel ? Color.white : new Color(1, 1, 1, 0);
        }
    }
}
