using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SystemSingleton<FXManager>
{
    private GameObject m_fxManager;
    private GameObject m_backTrackManager;
    private Dictionary<string, AudioClip> m_sfxCache;

    private Queue<AudioSource> m_sfxQueue;

    // Start is called before the first frame update
    void Start() //Only works with Awake()???
    {
        m_sfxCache = new Dictionary<string, AudioClip>();
        //Creates the AudioSource
        m_fxManager = new GameObject("MusicSource");
        m_fxManager.AddComponent<AudioSource>();
        m_backTrackManager = new GameObject("MusicSource");
        m_backTrackManager.AddComponent<AudioSource>();

        m_sfxQueue = new Queue<AudioSource>();
        for(int i = 0; i < 10; i++)
        {
            m_sfxQueue.Enqueue(m_fxManager.AddComponent<AudioSource>());
        }

        //Instantiate(m_fxManager);
        //Debug.Log("started FXManager");

        //Sets the music
        SetMusic("music/title_screen_theme_1");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Sets music to given filename
    public void SetMusic(string musicStr)
    {

        //Debug.Log(m_fxManager);
        //Debug.Log(musicStr);
        m_backTrackManager.GetComponent<AudioSource>().Play();

        if (GameManager.Get().m_debugMusicOff) return;


        AudioClip musicClip = Resources.Load<AudioClip>(musicStr);
        m_backTrackManager.GetComponent<AudioSource>().clip = musicClip;

        m_backTrackManager.GetComponent<AudioSource>().loop = true;
        m_backTrackManager.GetComponent<AudioSource>().Play();
    }

    public void PlaySFX(string sfxStr, float pitchBend = 0)
    {
        AudioClip sfx;
        if (!m_sfxCache.TryGetValue(sfxStr, out sfx))
        {
            sfx = Resources.Load<AudioClip>(sfxStr);
            m_sfxCache[sfxStr] = sfx;
        }

        var sfxChannel = m_sfxQueue.Dequeue();
        sfxChannel.pitch = 1 + pitchBend;
        sfxChannel.PlayOneShot(sfx);
        m_sfxQueue.Enqueue(sfxChannel);
    }
}
