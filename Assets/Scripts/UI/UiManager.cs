using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiManager : SystemSingleton<UiManager>
{
    private GameObject m_ui;
    private GameObject m_canvas;
    private GameObject m_mainMenuPanel;
    private GameObject m_gameplayPanel;
    private GameObject m_pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        m_ui = Instantiate(Resources.Load<GameObject>("UIPrefab"));
        var canvas = m_ui.transform.GetChild(0);
        m_mainMenuPanel = canvas.GetChild(0).gameObject;
        m_gameplayPanel = canvas.GetChild(1).gameObject;
        m_pausePanel = canvas.GetChild(2).gameObject;
        m_gameplayPanel.SetActive(false);
        m_pausePanel.SetActive(false);
    }

    public void ShowGameplayPanel()
    {
        m_mainMenuPanel.SetActive(false);  //Hides the main menu
        m_gameplayPanel.SetActive(true);  //Unhides the gameplay UI
    }

    public void ShowPausePanel(bool isPaused)
    {
        m_pausePanel.SetActive(isPaused);
    }

    public void UpdatePlayerHealth(int healthLevel)
    {
        m_ui.GetComponent<ButtonFuctions>().SetHealthlevel(healthLevel);
    }

    public void UpdateAmmoUi(int ammoLevel)
    {
        m_ui.GetComponent<ButtonFuctions>().SetAmmoLevel(ammoLevel);
    }

}
