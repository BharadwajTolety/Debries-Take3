using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class read_Score : MonoBehaviour
{
    StreamReader readScore;
    string scorePath;

    private void Awake()
    {
        scorePath = Application.dataPath + "/Database/Input/score_info_fromMatlab.txt";
        
        reset_score();
    }

    //reset score file so that matlab can rewrite it
    private void reset_score()
    {
        if(File.Exists(scorePath))
        {
            File.Delete(scorePath);
        }
        
        //reset/init the file to waiting..
        //File.WriteAllText(scorePath,"waiting...");

        //reset/init variables
        Manager.Instance.minTime = 0; Manager.Instance.maxProfit = 0;
    }

    //read the score file written by matlab algo server thingy
    private bool read_score()
    {
        string initRead;

        if(!File.Exists(scorePath))
        {
            return false;
        }
        else
        {
            try
            {
                initRead = File.ReadAllText(scorePath);
            }
            catch (Exception e)
            {
                Debug.Log("the file couldnt be read - " + e.Message);
                return false;
            }

            string[] scoreInfo = new string[2];

            scoreInfo = initRead.Split(',');

            Manager.Instance.maxProfit = float.Parse(scoreInfo[0]);
            Manager.Instance.minTime = float.Parse(scoreInfo[1]);
            return true;
        }
    }
     
    //keep reading the score file while matlab is calculating and done writing the score file
    public void reading()
    {
        //reset score file so it be waiting for matlab new scores
        reset_score();
        Debug.Log("waiting on score...");

        float timespent = 1f;
        while (!read_score())
        {
            timespent *= Time.unscaledDeltaTime;
            if (timespent > 5)
            {
                Debug.Log("Matlab taking too long something wrong - " + timespent);
                break;
            }
                
        };

        GameObject map = GameObject.FindGameObjectWithTag("ALLOBJECTS");
        badEdge_blinkers read_bE = (badEdge_blinkers)map.GetComponent(typeof(badEdge_blinkers));
        read_bE.read_badEdges();

        Debug.Log(Manager.Instance.maxProfit);
        Debug.Log(Manager.Instance.minTime);
    }
}
