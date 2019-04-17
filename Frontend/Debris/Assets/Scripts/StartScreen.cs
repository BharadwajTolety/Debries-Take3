using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class StartScreen : MonoBehaviour {

    public GameObject player_info, session_info, notification;
    public Dropdown dropdown_game;

    private void Awake()
    {
        //get the options setup
        DirectoryInfo d = new DirectoryInfo(Application.streamingAssetsPath + "/Database/Input/res_setup");
        FileInfo[] files = d.GetFiles("*.txt");

        List<string> dropOptions = new List<string>();
        foreach (FileInfo file in files)
        {
            string op_name = file.ToString().Substring(file.ToString().IndexOf("res_setup") + 10);
            dropOptions.Add(op_name);
        }

        dropdown_game.ClearOptions();
        dropdown_game.AddOptions(dropOptions);

        Manager.Instance.map_json = "";
    }

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

        string log_directory = Application.streamingAssetsPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId;

        if(!Directory.Exists(log_directory))
        {
            Directory.CreateDirectory(log_directory);
        }
        else
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(log_directory);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        SceneManager.LoadScene("MainGame");
    }
    public void ExitGame() 
    {
        Debug.Log("EXIT!");
        Application.Quit();
    }

    public void select_game()
    {
        string setup = Application.streamingAssetsPath + "/Database/Input/res_setup/setup_" + dropdown_game.value + ".txt";

        if(!File.Exists(setup))
        {
            notification.SetActive(true);
        }
        else
        {
            string[] lines = File.ReadAllLines(setup);

            bool flag = false;
            foreach(string line in lines)
            {
                if(!flag)
                    Manager.Instance.map_json = Application.streamingAssetsPath + "/Database/Input/Node_data_" + line + ".json";
                else
                    Manager.Instance.game_setup = Application.streamingAssetsPath + "/Database/Input/" + line + ".json";

                flag = true;
            }
        }
    }
}
