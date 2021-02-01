using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFuctions : MonoBehaviour
{
    [SerializeField] private Transform m_ammoPanel;
    [SerializeField] private Transform m_healthPanel;
    [SerializeField] private GameObject m_creditsPanel;

    private bool isPaused = false;

    public void ToggleStoryMode()
    {
        UiManager.Get().IsStoryEnabled = !UiManager.Get().IsStoryEnabled;
        Debug.Log("Story mode: " + UiManager.Get().IsStoryEnabled);
    }

    public void StartGameplay()
    {
        GameManager.Get().StartGameplay();
    }

    public void ExitApplication()
    {
        //Debug.Log("Quitting Application");
        Application.Quit();
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

    public void ToggleCredits()
    {
        m_creditsPanel.SetActive(!m_creditsPanel.activeSelf);
    }
}
