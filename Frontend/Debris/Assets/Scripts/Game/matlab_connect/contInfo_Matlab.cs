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
    
    private void Awake()
    {
        csvPath = Application.dataPath + "/Database/Output/edgelist_forMatlab.csv";
        write_CSV(csvPath);
    }

    //write out the csv for matlab to read
    void write_CSV(string path)
    {
        //if files doesn't exist make it
        if(!File.Exists(path))
        {
            File.WriteAllText(path, "-,-,-\n");
        }
        else
        {
            File.WriteAllText(path, string.Empty);
        }
    }

    //read all the contractor info and then write that info into csv
    public void read_contractor_info()
    {
        Manager.Instance.scans += 1;
        Manager.Instance.edge_changes = 0;

        int count_edges = write_map_csv(csvPath, false);

        Debug.Log("writting complete!! total edges - " + count_edges);

        //setup the client for the matlab server to read
        setupSocket();

        call_reading();
    }

    //write into csv file
    private int write_map_csv(string path, bool score)
    {
        File.WriteAllText(path, string.Empty);

        GameObject[] themRedEdges = GameObject.FindGameObjectsWithTag("redLine");
        GameObject[] themGreenEdges = GameObject.FindGameObjectsWithTag("greenLine");
        GameObject[] themBlueEdges = GameObject.FindGameObjectsWithTag("blueLine");

        string from = "-";
        string to = "-";
        string nc = "-";
        StringBuilder csv = new StringBuilder();
        string[] nodeInfo = new string[4];
        int count_edges = 0;

        //write down score on the first line if this is a scan file
        if (score)
        {
            csv.AppendLine("first time, three scores, then edgelist");

            float timeLog = Time.fixedUnscaledTime;
            csv.AppendLine(timeLog.ToString());

            for (int i = 0; i < 3; i++)
            {
                string cncline = string.Format("{0},{1},{2}", Manager.Instance.cncProfit[i], Manager.Instance.cncTime[i], (i+1));
                csv.AppendLine(cncline);
            }

            string newline = string.Format("{0},{1},Fullscore", Manager.Instance.maxProfit, Manager.Instance.minTime);
            csv.AppendLine(newline);
        }

        //read the current map and write that down onto csv
        for (int i = 0; i < themRedEdges.Length; i++)
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
                    File.WriteAllText(path, csv.ToString());
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
                    File.WriteAllText(path, csv.ToString());
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
                    File.WriteAllText(path, csv.ToString());
                    count_edges += 1;
                }
            }
        }

        return count_edges;

    }

    //read score sent from matlab
    private void call_reading()
    {
        GameObject gameManager = GameObject.Find("GameManager") ;
        read_Score read = (read_Score)gameManager.GetComponent(typeof(read_Score));

        //start reading
        read.reading();

        float maxProfit = Manager.Instance.maxProfit;
        float minTime = Manager.Instance.minTime;

        string exPath = Application.dataPath + "/Database/Output/P1_S1/Scan_" + Manager.Instance.scans + ".csv";
        write_map_csv(exPath , true);
    }
}