using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ver_control : MonoBehaviour
{
    public Button[] ver_buttons;

    private List<float> maxProfit = new List<float>(); //really this is minprofit
    private List<float> minTime = new List<float>();    //really this is max time

    private List<float>[] cncProfit;
    private List<float>[] cncTime;

    private string ver_directory;

    private List<float> intersect;
    private int index_lines;

    private Dictionary<string, string> map = new Dictionary<string, string>();

    private int ver_cont = 0;
    private int count_ver = 0;

    private void Awake()
    {
        ver_directory = Application.streamingAssetsPath + "/Database/Input/ver_cntrl";
        intersect = new List<float>();

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
        empty_folder(true);
    }

    public void empty_folder(bool restart = false)
    {
        if (!Directory.Exists(ver_directory))
        {
            Directory.CreateDirectory(ver_directory);
        }
        else
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(ver_directory);

            if (restart)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                restart_graphs();
                update_verList(true);
            }
            else
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    string s = file.Name.Substring(5,1);
                    int ver = (int.Parse(s));
                    if (ver > Manager.Instance.on_ver)
                        file.Delete();
                }

                reset_lists_forScan();
            }
        }
    }

    private void push_scoreList()
    {
        intersect.Add(Manager.Instance.intersect);
        maxProfit.Add(Manager.Instance.maxProfit);
        minTime.Add(Manager.Instance.minTime);

        for (int i = 0; i < 3; i++)
        {
            cncProfit[i].Add(Manager.Instance.cncProfit[i]);
            cncTime[i].Add(Manager.Instance.cncTime[i]);
        }
    }

    public void update_verList(bool restart = false)
    {
        if (!restart)
        {
            if (count_ver < Manager.Instance.scans)
            {
                count_ver++;
                if (ver_cont < 5)
                    ver_cont++;
            }

            //for (int i = (ver_cont - 1); i >= 0; i--)
            //{
            //    Button vers_button = ver_buttons[i];
            //    vers_button.interactable = true;
            //
            //    //keep the gameobject names static to calculate total based on size of lists for graphs
            //    //vers_button.gameObject.name = (Manager.Instance.scans - i).ToString();
            //
            //    //change the display of the button for user accessibility
            //    vers_button.gameObject.GetComponent<Text>().text = (Manager.Instance.scans - i).ToString();
            //}

            System.IO.DirectoryInfo di = new DirectoryInfo(ver_directory);
            int text_number = 0;
            for (int k = di.GetFiles().Length - 1; k >= 0; k--)
                if (di.GetFiles()[k].Name.EndsWith(".csv"))
                    text_number++;
                
            for (int i = 0, j = di.GetFiles().Length - 1; j >= 0 ; j--)
            {
                if (di.GetFiles()[j].Name.EndsWith(".csv"))
                {
                    ver_buttons[i].interactable = true;
                    ver_buttons[i].name = di.GetFiles()[j].Name.Substring(5, 1);
                    ver_buttons[i].gameObject.GetComponent<Text>().text = text_number.ToString();
                    text_number--;
                    i++;
                }
            }

            push_scoreList();
        }
        else
        {
            clear_lists_names();
        }
    }

    private void reset_lists_forScan()
    {
        ver_cont = 0;

        for (int i = 0, j  = ver_buttons.Length; i < ver_buttons.Length; i++, j--)
        {   
            Button vers_button = ver_buttons[i];
            vers_button.interactable = false;
            vers_button.gameObject.GetComponent<Text>().text = "-";
        }

        minTime.RemoveRange(index_lines,minTime.Count- index_lines);
        maxProfit.RemoveRange(index_lines,maxProfit.Count- index_lines);
        intersect.RemoveRange(index_lines,intersect.Count - index_lines);

        for (int i = 0; i < 3; i++)
        {
            cncProfit[i].RemoveRange(index_lines, cncProfit[i].Count - index_lines);
            cncTime[i].RemoveRange(index_lines, cncTime[i].Count - index_lines);
        }
    }

    private void clear_lists_names()
    {
        //reset everything for a new run
        count_ver = 0;
        ver_cont = 0;

        for (int i = 0,j = ver_buttons.Length; i < ver_buttons.Length; i++, j--)
        {
            ver_buttons[i].interactable = false;
            ver_buttons[i].gameObject.GetComponent<Text>().text = "-";
        }

        intersect.Clear();
        maxProfit.Clear();
        minTime.Clear();

        for (int i = 0; i < 3; i++)
        {
            cncProfit[i].Clear();
            cncTime[i].Clear();
        }
    }

    public void update_mapVer(GameObject ver)
    {
        string total = ver.tag.Substring(4,1);
        //the version name should only be the version number
        if (read_scanFile(ver.name))
        {
            Manager.Instance.on_ver = int.Parse(ver.name);
            mapBrushing.map_update_ver(map);
            reUpdate_graphs(int.Parse(total));
        }
    }

    //for when starting on a run
    public void restart_graphs()
    {
        GameObject.Find("current_total").GetComponent<LineChart>().reUpdate(null, null, true);
        GameObject.Find("current_profit").GetComponent<BarChart>().reUpdate(null, true);
        GameObject.Find("current_time").GetComponent<BarChart>().reUpdate(null, true);
        GameObject.Find("GameManager").GetComponent<graph_view>().destroy_inter_marks();
        GameObject.Find("GameManager").GetComponent<graph_view>().intersection_update(0f);
    }

    private void reUpdate_graphs(int index)
    {
        int total = maxProfit.Count - (index-1);
        index_lines = total;
        float[] profit_verCont = maxProfit.GetRange(0,total).ToArray();
        float[] time_verCont = minTime.GetRange(0, total).ToArray();

        GameObject.Find("current_total").GetComponent<LineChart>().reUpdate(time_verCont,profit_verCont);

        List<float> cncProfit_verCont = new List<float>();
        List<float> cncTime_verCont = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            float prft = cncProfit[i][total-1];
            cncProfit_verCont.Add(prft);

            float tme = cncTime[i][total-1];
            cncTime_verCont.Add(tme);
        }

        GameObject.Find("current_profit").GetComponent<BarChart>().reUpdate(cncProfit_verCont);
        GameObject.Find("current_time").GetComponent<BarChart>().reUpdate(cncTime_verCont);

        GameObject.Find("GameManager").GetComponent<graph_view>().intersection_update(intersect[total-1], true);
    }

    //this needs to be updated for multiple edge contractors
    private bool read_scanFile(string ver)
    {
        map.Clear();
        string scanFile = Application.streamingAssetsPath + "/Database/Input/ver_cntrl/Scan_" + (ver) + ".csv";

        if(!File.Exists(scanFile))
        {
            return false;
        }
        else
        {
            string[] readScan = File.ReadAllLines(scanFile);
            string[] scoreInfo = new string[3];

            for (int i = 0; i<readScan.Length;i++)
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
