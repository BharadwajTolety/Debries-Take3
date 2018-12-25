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


    public Dictionary<string, string>[] map_info = new Dictionary<string, string>[]
    {
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>(),
        new Dictionary<string, string>()
    };

    //save contractor information for the map 
    public void save_map(GameObject edge)
    {
        string[] nodeInfo = new string[4];

        if (map_version > 4)
        {
            Array.Copy(map_info, 1, map_info, 0, map_info.Length - 1);
            map_info[4] = new Dictionary<string, string>();
            map_version -= 1;
        }

        nodeInfo = edge.name.Split('_');
        if (nodeInfo.Length > 2)
        {
                switch (edge.tag)
                {
                    case "redLine":
                        map_info[map_version].Add(edge.name, "LineRed");
                        break;
                    case "blueLine":
                        map_info[map_version].Add(edge.name, "LineBlue");
                        break;
                    case "greenLine":
                        map_info[map_version].Add(edge.name, "LineGreen");
                        break;
                    default:
                        Debug.Log("not color line, some problem occured");
                        break;
                }
        }
    }
}