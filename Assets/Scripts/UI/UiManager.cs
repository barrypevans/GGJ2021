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

    private GameObject m_dialogs;
    public bool IsStoryEnabled = true;
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
        // story
        m_dialogs = canvas.GetChild(3).gameObject;
        m_dialogs.SetActive(false);
    }

    public void PlayStory(int story)
    {
        if(!IsStoryEnabled)
        {
            GameManager.Get().StoryCompleted(story);
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ShowDialogs(story));
    }

    private IEnumerator ShowDialogs(int story)
    {
        m_dialogs.SetActive(true);
        // first child is background image so add 1
        var sequence = m_dialogs.transform.GetChild(story+1).gameObject;
        for(int i = 0; i < sequence.transform.childCount; i++)
        {
            var dialog = sequence.transform.GetChild(i).gameObject;
            dialog.SetActive(true);
            yield return new WaitForSeconds(5);
            dialog.SetActive(false);
        }
        m_dialogs.SetActive(false);
        GameManager.Get().StoryCompleted(story);
        yield return null;
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
