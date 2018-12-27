using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    public int mySelection = 1;
    public float brushSize = 50;

    public List<float> debrisList;
    public List<float> TimesList;

    public int map_version = 0;
    public bool flag = false; //flag checking whether map coloring has been changed or not

    public int scans = 0; //number of scans done
    public float maxProfit, minTime; //the scores for the game

    //the scores for individual contractors
    public float[] cncProfit = new float[3];
    public float[] cncTime = new float[3];

    public List<Dictionary<string, string>> map_info = new List<Dictionary<string, string>>();
    /*
    {
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>()
    };*/

    //save contractor information for the map 
    public void save_map(int map_ver, GameObject edge)
    {
        string[] nodeInfo = new string[4];

        //we only want 5 versions of map save
        if (map_ver > 4)
        {
            //Array.Copy(map_info, 1, map_info, 0, map_info.Count - 1);
            for(int i = 1; i <= 4; i ++)
            {
                map_info[i - 1] = map_info[i];
            }
                
            map_info.RemoveAt(4);
            map_ver -= 1;
        }

        nodeInfo = edge.name.Split('_');
        if (nodeInfo.Length > 2)
        {
            //intialise them dictionaries for current saves only
            if (map_info.Count <= map_ver)
                map_info.Add(new Dictionary<string, string>());

            if (map_info.Count>map_ver && Input.GetMouseButtonUp(0))
            {
                //delete map info for older map versions existing over the current save version
                for (int i = map_info.Count - 1; i > map_ver; i--)
                {
                    map_info.RemoveAt(i);
                }
            }

            //delete present keys when needed - correction for when you overwrite colors on the same turn
            if (map_info[map_ver].ContainsKey(edge.name))
            {
                map_info[map_ver].Remove(edge.name);
            }

            //save current map version
            switch (edge.tag)
            {
                case "redLine":
                    map_info[map_ver].Add(edge.name, "LineRed");
                    break;
                case "blueLine":
                    map_info[map_ver].Add(edge.name, "LineBlue");
                    break;
                case "greenLine":
                    map_info[map_ver].Add(edge.name, "LineGreen");
                    break;
                default:
                    Debug.Log("not color line, some problem occured");
                    break;
            }
        }
    }
}