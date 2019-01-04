using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class StartScreen : MonoBehaviour {

    public GameObject player_info, session_info;

    public void StartGame()
    {
        if(player_info.GetComponent<InputField>().text == "" || session_info.GetComponent<InputField>().text == "")
        {
            Manager.Instance.playerId   =   "def";
            Manager.Instance.sessionId  =   "def";
        }
        else
        {
            Manager.Instance.playerId = player_info.GetComponent<InputField>().text;
            Manager.Instance.sessionId = session_info.GetComponent<InputField>().text;
        }

        string log_directory = Application.dataPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId;

        if(!Directory.Exists(log_directory))
        {
            Directory.CreateDirectory(log_directory);
        }

        SceneManager.LoadScene("MainGame");
    }
    public void ExitGame() 
    {
        Debug.Log("EXIT!");
        Application.Quit();
    }
}
