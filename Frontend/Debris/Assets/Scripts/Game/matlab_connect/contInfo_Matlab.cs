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
    string[] readFile;
    int count_edges = 0;

    private void Awake()
    {
        count_edges = 0;

        csvPath = Application.dataPath + "/Database/Output/edgelist_forMatlab.csv";
        write_CSV(csvPath);

        if (Manager.Instance.sessionId == "" || Manager.Instance.playerId == "")
        {
            Manager.Instance.playerId = "def";
            Manager.Instance.sessionId = "def";

            string log_directory = Application.dataPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId;

            System.IO.DirectoryInfo di = new DirectoryInfo(log_directory);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
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
        if ((themWhiteEdges.Length == 1 && themWhiteEdges[0].name == "white") || Manager.Instance.scans > 0)
        {
            Manager.Instance.scans += 1;
            Manager.Instance.edge_changes = 0;

            int count_edges = write_map_csv(csvPath, false, profit, time, intersect);

            Debug.Log("writting complete!! total edges - " + count_edges);

            //setup the client for the matlab server to read
            setupSocket();

            call_reading(profit, time, intersect);
        }
    }

    public void write_log(string path, int profit_obj, int time_obj, int intersect_obj)
    {
        write_map_csv(path, true, profit_obj, time_obj, intersect_obj);
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
            List<GameObject[]> lines_ = new List<GameObject[]>
            {
                themRedEdges, themGreenEdges, themBlueEdges, red_blue, red_green, green_blue, all_color
            };

        StringBuilder csv = new StringBuilder();
        count_edges = 0;
        string csv_input;

        //write the scores in for log
        if(score)
        {
            csv_input = write_scores();
            if (csv_input.EndsWith("\r\n"))
            {
                csv_input = csv_input.Substring(0, csv_input.Length - 2);
                csv.AppendLine(csv_input);
            }

            string scanFile = Application.dataPath + "/Database/Input/brushedEdges_Matlab.csv";

            if (!File.Exists(scanFile))
            {
                score = false;
            }
            else
            {
                readFile = File.ReadAllLines(scanFile);
            }
        }

        //putting up obj category info
        string objline = string.Format("{0},{1},{2}", profit_obj, time_obj, intersect_obj);
        csv.AppendLine(objline);

        //write edges here
        foreach (GameObject[] lines in lines_)
        {
            csv_input = write_edges(path, lines, score);
            if (csv_input.EndsWith("\r\n"))
            {
                csv_input = csv_input.Substring(0, csv_input.Length - 2);
                csv.AppendLine(csv_input);
            }
        }

        File.WriteAllText(path, csv.ToString());
        return count_edges;
    }

    private string write_scores()
    {
        StringBuilder csv = new StringBuilder();
        string[] nodeInfo = new string[4];

        csv.AppendLine("first time, three scores, then objinputs+edgelist");

        float timeLog = Time.fixedUnscaledTime;
        csv.AppendLine(timeLog.ToString());

        for (int i = 0; i < 3; i++)
        {
            string cncline = string.Format("{0},{1},{2}", Manager.Instance.cncProfit[i], Manager.Instance.cncTime[i], (i + 1));
            csv.AppendLine(cncline);
        }

        string newline = string.Format("{0},{1},Fullscore", Manager.Instance.maxProfit, Manager.Instance.minTime);
        csv.AppendLine(newline);

        return csv.ToString();
    }

    private string write_edges(string path, GameObject[] edges, bool score)
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
                    string newline;
                    if (!score)
                    {
                        newline = string.Format("{0},{1},{2}", from, to, nc);
                    }
                    else
                    {
                        string[] scoreInfo = new string[3];
                        scoreInfo = readFile[count_edges + 1].Split(',');
                        newline = string.Format("{0},{1},{2},{3},{4},{5},{6}", from, to, nc, "-", scoreInfo[0], scoreInfo[1], scoreInfo[2]);
                    }
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
        read.reading(profit_obj, time_obj, intersect_obj);   
    }
}