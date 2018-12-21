using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Text;

public class contInfo_Matlab : classSocket
{
    string csvPath;
    GameObject gameManager;

    private void Awake()
    {
       write_CSV();
    }

    //write out the csv for matlab to read
    void write_CSV()
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

    //read all the contractor info and then write that info into csv
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
        string[] nodeInfo = new string[4];
        int count_edges = 0;

        for(int i = 0 ; i < themRedEdges.Length ; i++)
        {
            nodeInfo = themRedEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                from = nodeInfo[2];
                to = nodeInfo[3];
                nc = "1";

                if (from != "-" || to != "-" || nc != "-")
                {
                    string newline = string.Format("{0},{1},{2}", from, to, nc);
                    csv.AppendLine(newline);
                    File.WriteAllText(csvPath, csv.ToString());
                    count_edges += 1;
                }
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

                if (from != "-" || to != "-" || nc != "-")
                {
                    string newline = string.Format("{0},{1},{2}", from, to, nc);
                    csv.AppendLine(newline);
                    File.WriteAllText(csvPath, csv.ToString());
                    count_edges += 1;
                }
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

                if (from != "-" || to != "-" || nc != "-")
                {
                    string newline = string.Format("{0},{1},{2}", from, to, nc);
                    csv.AppendLine(newline);
                    File.WriteAllText(csvPath, csv.ToString());
                    count_edges += 1;
                }
            }
        }

        Debug.Log("writting complete!! total edges - " + count_edges);

        //setup the client for the matlab server to read
        setupSocket();

        call_reading();
    }

    /*
    private void call_mat()
    {
        //make a matlab instance
        MLApp.MLApp matlab = new MLApp.MLApp();

        //get the codes library in
        matlab.Execute(@"cd C:\Users\Uttkarsh\Desktop\Debris_work_folder\Codes-debris\Codes");

        object mlab_result = null;

        matlab.Feval("unity_start", 0, out mlab_result);
    }
    */

    //read score sent from matlab
    private void call_reading()
    {
        gameManager = GameObject.Find("GameManager") ;
        read_Score read = (read_Score)gameManager.GetComponent(typeof(read_Score));

        //start reading
        read.reading();
    }
}