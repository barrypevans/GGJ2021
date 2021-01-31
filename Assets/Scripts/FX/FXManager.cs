using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SystemSingleton<FXManager>
{
    private GameObject m_fxManager;

    // Start is called before the first frame update
    void Start() //Only works with Awake()???
    {
        //Creates the AudioSource
        m_fxManager = new GameObject("MusicSource");
        m_fxManager.AddComponent<AudioSource>();
        //Instantiate(m_fxManager);
        //Debug.Log("started FXManager");

        //Sets the music
        SetMusic("title_screen_theme_1");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Sets music to given filename
    public void SetMusic(string musicStr)
    {

        Debug.Log(m_fxManager);
        Debug.Log(musicStr);
        m_fxManager.GetComponent<AudioSource>().Play();

        if (GameManager.Get().m_debugMusicOff) return;


        AudioClip musicClip = Resources.Load<AudioClip>(musicStr);
        m_fxManager.GetComponent<AudioSource>().clip = musicClip;

        m_fxManager.GetComponent<AudioSource>().loop = true;
        m_fxManager.GetComponent<AudioSource>().Play();
    }
}
