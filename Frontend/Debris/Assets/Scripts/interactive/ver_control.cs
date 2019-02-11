using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ver_control : mapBrushing {

    private List<float> maxProfit   = new List<float>();
    private List<float> minTime     = new List<float>();

    private List<float>[] cncProfit  = new List<float>[3];
    private List<float>[] cncTime    = new List<float>[3];

    private Dictionary<string, string> map = new Dictionary<string, string>();

    private int ver_cont = 0;
    private int count_ver = 0;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject.FindGameObjectWithTag("ver_" + (i + 1)).GetComponent<Button>().interactable = false;
        }
    }

    private void push_scoreList()
    {
        maxProfit.Add (Manager.Instance.maxProfit);
        minTime.Add(Manager.Instance.minTime);

        for (int i = 0; i < 3; i++)
        {
            cncProfit[i] = new List<float>();
            cncTime[i] = new List<float>();

            cncProfit[i].Add(Manager.Instance.cncProfit[i]);
            cncTime[i].Add(Manager.Instance.cncTime[i]);
        }
    }

    public void update_verList()
    {
        if (count_ver < Manager.Instance.scans)
        {
            count_ver++;
            if (ver_cont <= 5)
                ver_cont++;
        }

        for(int i = 0, j = count_ver-1; i<ver_cont; i++, j--)
        {
            GameObject vers_button = GameObject.FindGameObjectWithTag("ver_" + (i + 1));
            vers_button.GetComponent<Button>().interactable = true;
            vers_button.name = (Manager.Instance.scans - j).ToString();
        }
        push_scoreList();
    }

    public void update_mapVer(GameObject ver)
    {
        //the version name should only be the version number
        if(read_scanFile(ver.name))
        {
            map_update_ver(map);
            reUpdate_graphs(int.Parse(ver.name));
        }
    }

    //fix this
    private void reUpdate_graphs(int total)
    {
        float[] profit_verCont = maxProfit.GetRange(0,total).ToArray();
        float[] time_verCont = minTime.GetRange(0, total).ToArray();

        GameObject.Find("score_total").GetComponent<LineChart>().reUpdate(profit_verCont, time_verCont);

        for (int i = 0; i < 3; i++)
        {
            float[] cncProfit_verCont = cncProfit[i].GetRange(0, total).ToArray();
            float[] cncTime_verCont = cncTime[i].GetRange(0, total).ToArray();

            GameObject.Find("profit_" + (i+1)).GetComponent<BarChart>().reUpdate(cncProfit_verCont);
            GameObject.Find("time_" + (i+1)).GetComponent<BarChart>().reUpdate(cncTime_verCont);
        }
    }

    //this needs to be updated for multiple edge contractors
    private bool read_scanFile(string ver)
    {
        map.Clear();
        string scanFile = Application.dataPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId + "/Scan_" + ver + ".csv";

        if(!File.Exists(scanFile))
        {
            return false;
        }
        else
        {
            string[] readScan = File.ReadAllLines(scanFile);
            string[] scoreInfo = new string[3];

            for (int i = 6; i<readScan.Length;i++)
            {
                scoreInfo =  readScan[i].Split(',');
                switch(int.Parse(scoreInfo[2]))
                {
                    case 1:
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "red");
                        break;
                    case 2:
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "green");
                        break;
                    case 3:
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "blue");
                        break;
                    default:
                        Debug.Log("prob in ver control read scanfile, white line maybe");
                        return false;
                }
            }
            return true;
        }
    }
}
