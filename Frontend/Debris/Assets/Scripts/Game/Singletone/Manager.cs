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

    public Dictionary<string, string>[] map_info = new Dictionary<string, string>[]
    {
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>()
    };

    //save contractor information for the map 
    public void save_map(int map_ver, GameObject edge)
    {
        string[] nodeInfo = new string[4];

        if (map_ver > 4)
        {
            Array.Copy(map_info, 1, map_info, 0, map_info.Length - 1);
            map_info[4] = new Dictionary<string, string>();
            map_ver -= 1;
        }

        nodeInfo = edge.name.Split('_');
        if (nodeInfo.Length > 2)
        {
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