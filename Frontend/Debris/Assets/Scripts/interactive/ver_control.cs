using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ver_control : MonoBehaviour
{
    public Button[] ver_buttons;

    private List<float> maxProfit   = new List<float>();
    private List<float> minTime     = new List<float>();

    private List<float>[] cncProfit;
    private List<float>[] cncTime;

    private Dictionary<string, string> map = new Dictionary<string, string>();

    private int ver_cont = 0;
    private int count_ver = 0;

    private void Awake()
    {
        cncProfit = new List<float>[3];
        cncTime = new List<float>[3];

        for (int i = 0; i < 3; i++)
        {
            cncProfit[i] = new List<float>();
            cncTime[i] = new List<float>();
        }

        for (int i = 0; i < 5; i++)
        {
            ver_buttons[i].interactable = false;
        }

        //empty the folder for a new game
        empty_folder();
    }

    public void empty_folder(bool restart = false)
    {
        string log_directory = Application.streamingAssetsPath + "/Database/Input/ver_cntrl";
        if (!Directory.Exists(log_directory))
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

        if(restart)
        {
            update_verList(restart);
            restart_graphs();
        }
    }

    private void push_scoreList()
    {
        maxProfit.Add (Manager.Instance.maxProfit);
        minTime.Add(Manager.Instance.minTime);

        for (int i = 0; i < 3; i++)
        {
            cncProfit[i].Add(Manager.Instance.cncProfit[i]);
            cncTime[i].Add(Manager.Instance.cncTime[i]);
        }
    }

    public void update_verList(bool restart = false)
    {
        if(!restart)
        {
            if (count_ver < Manager.Instance.scans)
            {
                count_ver++;
                if (ver_cont < 5)
                    ver_cont++;
            }

            for (int i = 0, j = count_ver - 1; i < ver_cont; i++, j--)
            {
                Button vers_button = ver_buttons[i];
                vers_button.interactable = true;
                vers_button.gameObject.name = (Manager.Instance.scans - j).ToString();

                //change the display of the button for user accessibility
                vers_button.gameObject.GetComponent<Text>().text = vers_button.name;
            }
            push_scoreList();
        }
        else
        {
            //reset everything for a new run
            count_ver = 0;
            ver_cont = 0;

            for (int i = 0; i < 5; i++)
            {
                ver_buttons[i].interactable = false;
            }

            maxProfit.Clear();
            minTime.Clear();

            for (int i = 0; i < 3; i++)
            {
                cncProfit[i].Clear();
                cncTime[i].Clear();
            }
        }
       
    }

    public void update_mapVer(GameObject ver)
    {
        //the version name should only be the version number
        if(read_scanFile(ver.name))
        {
            mapBrushing.map_update_ver(map);
            reUpdate_graphs(int.Parse(ver.name));
        }
    }

    //for when starting on a run
    public void restart_graphs()
    {
        GameObject.Find("current_total").GetComponent<LineChart>().reUpdate(null, null, true);
        GameObject.Find("current_profit").GetComponent<BarChart>().reUpdate(null, true);
        GameObject.Find("current_time").GetComponent<BarChart>().reUpdate(null, true);
    }

    //fix this
    private void reUpdate_graphs(int total)
    {
        float[] profit_verCont = maxProfit.GetRange(0,total).ToArray();
        float[] time_verCont = minTime.GetRange(0, total).ToArray();

        GameObject.Find("current_total").GetComponent<LineChart>().reUpdate(profit_verCont, time_verCont);

        List<float> cncProfit_verCont = new List<float>();
        List<float> cncTime_verCont = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            cncProfit_verCont.Add(cncProfit[i].IndexOf(total));
            cncTime_verCont.Add(cncTime[i].IndexOf(total));
        }

        GameObject.Find("current_profit").GetComponent<BarChart>().reUpdate(cncProfit_verCont);
        GameObject.Find("current_time").GetComponent<BarChart>().reUpdate(cncTime_verCont);
    }

    //this needs to be updated for multiple edge contractors
    private bool read_scanFile(string ver)
    {
        map.Clear();
        string scanFile = Application.streamingAssetsPath + "/Database/Output/Input/ver_cntrl/Scan_" + (ver) + ".csv";

        if(!File.Exists(scanFile))
        {
            return false;
        }
        else
        {
            string[] readScan = File.ReadAllLines(scanFile);
            string[] scoreInfo = new string[3];

            for (int i = 7; i<readScan.Length;i++)
            {
                scoreInfo =  readScan[i].Split(',');
                switch(scoreInfo[2])
                {
                    case "1":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "red");
                        break;
                    case "2":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "green");
                        break;
                    case "3":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "blue");
                        break;
                    case "13":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "red+blue");
                        break;
                    case "23":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "green+blue");
                        break;
                    case "12":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "red+green");
                        break;
                    case "123":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "red+blue+green");
                        break;
                    case "":
                        map.Add("_" + scoreInfo[0] + "_" + scoreInfo[1], "white");
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
