using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SystemSingleton<FXManager>
{
    private GameObject m_fxManager;

    // Start is called before the first frame update
    void Awake()
    {
        //Creates the AudioSource
        m_fxManager = new GameObject("MusicSource");
        m_fxManager.AddComponent<AudioSource>();

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
        AudioClip musicClip = Resources.Load<AudioClip>(musicStr);
        m_fxManager.GetComponent<AudioSource>().clip = musicClip;

        m_fxManager.GetComponent<AudioSource>().loop = true;
        m_fxManager.GetComponent<AudioSource>().Play();
    }
}
