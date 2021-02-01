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
    private GameObject m_winPanel;
    private GameObject m_losePanel;

    private GameObject m_dialogs;
    public bool IsStoryEnabled = true;

    private GameObject[] m_waveCards;

    private bool dialogsRunning = false; //Flag for whether there is dialog running

    // Start is called before the first frame update
    void Start()
    {
        m_ui = Instantiate(Resources.Load<GameObject>("UIPrefab"));
        var canvas = m_ui.transform.GetChild(0);
        m_mainMenuPanel = canvas.GetChild(0).gameObject;
        m_gameplayPanel = canvas.GetChild(1).gameObject;
        m_pausePanel = canvas.GetChild(2).gameObject;
        m_winPanel = canvas.GetChild(4).gameObject;
        m_losePanel = canvas.GetChild(5).gameObject;

        m_gameplayPanel.SetActive(false);
        m_pausePanel.SetActive(false);
        m_winPanel.SetActive(false);
        m_losePanel.SetActive(false);
        // story
        m_dialogs = canvas.GetChild(3).gameObject;
        m_dialogs.SetActive(false);
        // wave cards
        //var waveCardParent = canvas.GetChild(6).gameObject;
        //m_waveCards = new GameObject[waveCardParent.transform.childCount];
        //for(int i = 0; i < m_waveCards.Length; i++)
        //{
        //    m_waveCards[i] = waveCardParent.transform.GetChild(i).gameObject;
        //}
    }

    public void PlayStory(int story)
    {
        StopAllCoroutines();
        if (!IsStoryEnabled)
        {
            StartCoroutine(ShowWaveCard(story));
            return;
        }
        StartCoroutine(ShowDialogs(story));
    }

    private IEnumerator ShowWaveCard(int card)
    {
        if (m_waveCards != null && card < 3)
        {
            m_waveCards[card].SetActive(true);
            yield return new WaitForSeconds(1);
            m_waveCards[card].SetActive(false);
        }
        GameManager.Get().StoryCompleted(card);
        yield return null;
    }

    private IEnumerator ShowDialogs(int story)
    {
        dialogsRunning = true;
        m_dialogs.SetActive(true);
        // first child is background image so add 1
        var sequence = m_dialogs.transform.GetChild(story+1).gameObject;
        for(int i = 0; i < sequence.transform.childCount; i++)
        {
            var dialog = sequence.transform.GetChild(i).gameObject;
            dialog.SetActive(true);
            yield return new WaitForSeconds(3);
            dialog.SetActive(false);
        }

        if (m_waveCards != null && story < 3)
        {
            m_waveCards[story].SetActive(true);
            yield return new WaitForSeconds(1);
            m_waveCards[story].SetActive(false);
        }

        m_dialogs.SetActive(false);
        GameManager.Get().StoryCompleted(story);
        dialogsRunning = false;
        yield return null;
    }

    public void ShowGameplayPanel()
    {
        m_mainMenuPanel.SetActive(false);  //Hides the main menu
        m_gameplayPanel.SetActive(true);  //Unhides the gameplay UI
    }

    //Hides the dialoge panel if the game is paused, brings it back up if game resumes
    public void PauseResumeDialog(bool isPaused)
    {
        //Only works if dialog is currently being run.    
        if(dialogsRunning)
            m_dialogs.SetActive(isPaused);
    }

    public void ShowPausePanel(bool isPaused)
    {
        m_pausePanel.SetActive(isPaused);
    }

    public void ShowWinPanel(bool isShowing)
    {
        m_winPanel.SetActive(isShowing);
    }

    public void ShowLosePanel(bool isShowing)
    {
        m_losePanel.SetActive(isShowing);
    }

    public void UpdatePlayerHealth(int healthLevel)
    {
        m_ui.GetComponent<ButtonFuctions>().SetHealthlevel(healthLevel);
    }

    public void UpdateAmmoUi(int ammoLevel)
    {
        m_ui.GetComponent<ButtonFuctions>().SetAmmoLevel(ammoLevel);
    }

    public void SetWaveImage(int wave)
    {
        m_ui.GetComponent<ButtonFuctions>().SetWave(wave);
    }
}
