using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFuctions : MonoBehaviour
{
    //private GameObject uiPrefab;
    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //uiPrefab = GameObject.Find("UIPrefab");
        gameManager = GameObject.Find("game-manager");
    }

    /*public void PlayGame()
    {
        //Debug.Log("Playing Game");
        //Start playing the game somehow - need to coordinate
    }*/

    public void ExitApplication()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
