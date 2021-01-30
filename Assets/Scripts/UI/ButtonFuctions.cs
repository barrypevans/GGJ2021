using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFuctions : MonoBehaviour
{
    private GameObject uiPrefab;

    // Start is called before the first frame update
    void Start()
    {
        uiPrefab = GameObject.Find("UIPrefab");
    }

    public void PlayGame()
    {
        Debug.Log("Playing Game");
        //Start playing the game somehow - need to coordinate

        uiPrefab.SetActive(false); //optional way of hiding ui
        //May be handled by something else
    }

    public void ExitApplication()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
