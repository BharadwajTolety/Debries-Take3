using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ExitGame() 
    {
        Debug.Log("EXIT!");
        Application.Quit();
    }
}
