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
    int count_edges = 0;

    private void Awake()
    {
        count_edges = 0;

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
    public void read_contractor_info(int profit, int time, int intersect)
    {
        GameObject[] themWhiteEdges = GameObject.FindGameObjectsWithTag("white");
        if (themWhiteEdges.Length == 1 && themWhiteEdges[0].name == "white")
        {
            Manager.Instance.scans += 1;
            Manager.Instance.edge_changes = 0;

            int count_edges = write_map_csv(csvPath, false, profit, time, intersect);

            Debug.Log("writting complete!! total edges - " + count_edges);

            //setup the client for the matlab server to read
            setupSocket();

            call_reading(profit, time, intersect);

            update_graphs();

            update_verCont();
        }
    }

    //write into csv file
    private int write_map_csv(string path, bool score, int profit_obj, int time_obj, int intersect_obj)
    {
        File.WriteAllText(path, string.Empty);

        GameObject[] themRedEdges = GameObject.FindGameObjectsWithTag("red");
        GameObject[] themGreenEdges = GameObject.FindGameObjectsWithTag("green");
        GameObject[] themBlueEdges = GameObject.FindGameObjectsWithTag("blue");
        GameObject[] red_blue = GameObject.FindGameObjectsWithTag("red+blue");
        GameObject[] red_green = GameObject.FindGameObjectsWithTag("red+green");
        GameObject[] green_blue = GameObject.FindGameObjectsWithTag("green+blue");
        GameObject[] all_color = GameObject.FindGameObjectsWithTag("AllColor");

        StringBuilder csv = new StringBuilder();
        count_edges = 0;

        //write down score on the first line if this is a scan file
        if (score)
        {
            csv.AppendLine("first time, three scores, then objinputs+edgelist");

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

        //putting up obj category info
        string objline = string.Format("{0},{1},{2}", profit_obj, time_obj, intersect_obj);
        csv.AppendLine(objline);

        string csv_input;
        //write edges here
        csv_input = write_edges(path , themRedEdges);
        if (csv_input.EndsWith("\r\n"))
        {
            csv_input = csv_input.Substring(0, csv_input.Length - 2);
            csv.AppendLine(csv_input);
        }

        csv_input = write_edges(path , themGreenEdges);
        if (csv_input.EndsWith("\r\n"))
        {
            csv_input = csv_input.Substring(0, csv_input.Length - 2);
            csv.AppendLine(csv_input);
        }

        csv_input = write_edges(path , themBlueEdges);
        if (csv_input.EndsWith("\r\n"))
        {
            csv_input = csv_input.Substring(0, csv_input.Length - 2);
            csv.AppendLine(csv_input);
        }

        csv_input = write_edges(path , red_blue);
        if (csv_input.EndsWith("\r\n"))
        {
            csv_input = csv_input.Substring(0, csv_input.Length - 2);
            csv.AppendLine(csv_input);
        }

        csv_input = write_edges(path , red_green);
        if (csv_input.EndsWith("\r\n"))
        {
            csv_input = csv_input.Substring(0, csv_input.Length - 2);
            csv.AppendLine(csv_input);
        }

        csv_input = write_edges(path , green_blue);
        if (csv_input.EndsWith("\r\n"))
        {
            csv_input = csv_input.Substring(0, csv_input.Length - 2);
            csv.AppendLine(csv_input);
        }

        csv_input = write_edges(path , all_color);
        if (csv_input.EndsWith("\r\n"))
        {
            csv_input = csv_input.Substring(0, csv_input.Length - 2);
            csv.AppendLine(csv_input);
        }

        File.WriteAllText(path, csv.ToString());
        return count_edges;
    }

    private string write_edges(string path, GameObject[] edges)
    {
        string from = "-";
        string to = "-";
        string nc = "";
        StringBuilder csv = new StringBuilder();
        string[] nodeInfo = new string[4];

        if (edges[0].name.Contains("red"))
            nc += "1";
        if (edges[0].name.Contains("green"))
            nc += "2";
        if (edges[0].name.Contains("blue"))
            nc += "3";

        for (int i = 0; i < edges.Length; i++)
        {
            nodeInfo = edges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                from = nodeInfo[2];
                to = nodeInfo[3];

                if (from != "-" || to != "-")
                {
                    string newline = string.Format("{0},{1},{2}", from, to, nc);
                    csv.AppendLine(newline);
                    count_edges += 1;
                }
            }
        }

        return csv.ToString();
    }

    //read score sent from matlab
    private void call_reading(int profit_obj, int time_obj, int intersect_obj)
    {
        GameObject gameManager = GameObject.Find("GameManager") ;
        read_Score read = (read_Score)gameManager.GetComponent(typeof(read_Score));

        //start reading
       // read.reading();

        float maxProfit = Manager.Instance.maxProfit;
        float minTime = Manager.Instance.minTime;

        string exPath = Application.dataPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId + "/Scan_" + Manager.Instance.scans + ".csv";
        write_map_csv(exPath , true, profit_obj, time_obj, intersect_obj);
    }

    private void update_graphs()
    {
        GameObject.Find("profit_total").GetComponent<LineChart>().update_graph();
        GameObject.Find("time_total").GetComponent<LineChart>().update_graph();

        for(int i = 1; i < 4; i++)
        {
            GameObject.Find("profit_" + i).GetComponent<BarChart>().update_graph();
            GameObject.Find("time_" + i).GetComponent<BarChart>().update_graph();
        }
    }

    //just update version control scores every scan
    private void update_verCont()
    {
        GameObject.Find("ver_control").GetComponent<ver_control>().update_verList();
    }
}