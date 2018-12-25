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

    public int count_move = 0;
    public Dictionary<int, string>[] map_info = new Dictionary<int, string>[]
    {
        new Dictionary<int, string>(),
        new Dictionary<int, string>(),
        new Dictionary<int, string>(),
        new Dictionary<int, string>(),
        new Dictionary<int, string>()
    };

    //save contractor information for the map 
    public void save_map(int map_ver)
    {
        GameObject[] redEdges = GameObject.FindGameObjectsWithTag("redLine");
        GameObject[] greenEdges = GameObject.FindGameObjectsWithTag("greenLine");
        GameObject[] blueEdges = GameObject.FindGameObjectsWithTag("blueLine");

        string[] nodeInfo = new string[4];

        if (map_ver > 4)
        {
            Array.Copy(map_info,1,map_info,0,map_info.Length - 1);
            map_info[4] = new Dictionary<int, string>();
            count_move -= 1;
        }

        for (int i = 0; i < redEdges.Length; i++)
        {
            nodeInfo = redEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                map_info[map_ver].Add(1,nodeInfo[2]+nodeInfo[3]);
            }
        }

        for (int i = 0; i < greenEdges.Length; i++)
        {
            nodeInfo = greenEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                map_info[map_ver].Add(2, nodeInfo[2] + nodeInfo[3]);
            }
        }

        for (int i = 0; i < blueEdges.Length; i++)
        {
            nodeInfo = blueEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                map_info[map_ver].Add(3, nodeInfo[2] + nodeInfo[3]);
            }
        }
    }
}