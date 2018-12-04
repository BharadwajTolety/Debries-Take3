using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System;
using System.IO;
using System.Text;

public class contInfo_Matlab : classSocket
{
    string csvPath;

    private void Start()
    {
        writeCSV();
    }

    void writeCSV()
    {
        csvPath = Application.dataPath + "/Database/Output/edgelist_forMatlab.csv";

        //if files doesn't exist make it
        if(!File.Exists(csvPath))
        {
            File.WriteAllText(csvPath, "-,-,-\n");
        }
        else
        {
            File.WriteAllText(csvPath, string.Empty);
        }
    }

    public void read_contractor_info()
    {
        File.WriteAllText(csvPath, string.Empty);

        GameObject[] themRedEdges = GameObject.FindGameObjectsWithTag("redLine");
        GameObject[] themGreenEdges = GameObject.FindGameObjectsWithTag("greenLine");
        GameObject[] themBlueEdges = GameObject.FindGameObjectsWithTag("blueLine");

        string from = "-";
        string to = "-";
        string nc = "-";
        StringBuilder csv = new StringBuilder();
        string[] nodeInfo = new string[3];

        for(int i = 0 ; i < themRedEdges.Length ; i++)
        {
            nodeInfo = themRedEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                from = nodeInfo[2];
                to = nodeInfo[3];
                nc = "1";
            }

            if(from != "-" || to != "-" || nc !="-")
            {
                string newline = string.Format("{0},{1},{2}", from, to, nc);
                csv.AppendLine(newline);
                File.WriteAllText(csvPath, csv.ToString());
            }
        }

        for (int i = 0; i < themGreenEdges.Length; i++)
        {
            nodeInfo = themGreenEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                from = nodeInfo[2];
                to = nodeInfo[3];
                nc = "2";
            }

            if (from != "-" || to != "-" || nc != "-")
            {
                string newline = string.Format("{0},{1},{2}", from, to, nc);
                csv.AppendLine(newline);
                File.WriteAllText(csvPath, csv.ToString());
            }
        }

        for (int i = 0; i < themBlueEdges.Length; i++)
        {
            nodeInfo = themBlueEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                from = nodeInfo[2];
                to = nodeInfo[3];
                nc = "3";
            }

            if (from != "-" || to != "-" || nc != "-")
            {
                string newline = string.Format("{0},{1},{2}", from, to, nc);
                csv.AppendLine(newline);
                File.WriteAllText(csvPath, csv.ToString());
            }
        }

        Console.WriteLine("write!!");
    }

    private int Get_Node_number_from_string(string value, int i)
    {
        char delimiter = '_';
        string[] substrings = value.Split(delimiter);
        if (i >= substrings.Length)
        {
            return -1;
        }
        else
        {
            return System.Int32.Parse(substrings[i]);
        }
    }
}