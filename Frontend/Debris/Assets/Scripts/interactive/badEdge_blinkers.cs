using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class badEdge_blinkers : MonoBehaviour {

    private List<string>[] bad_edges = new List<string>[3];
    private bool on = true;
    private string[] badEdge_path;

    private void Awake()
    {
        //there are three badedge cases: time, profit and intersection
        badEdge_path[0] = Application.streamingAssetsPath + "/Database/Input/badEdges_from_Matlab.csv";
        badEdge_path[1] = Application.streamingAssetsPath + "/Database/Input/badEdges_from_Matlab_2.csv";
        badEdge_path[2] = Application.streamingAssetsPath + "/Database/Input/badEdges_from_Matlab_3.csv";

        reset_badEdge();
    }

    //delete badEdge file so that matlab can rewrite it
    private void reset_badEdge()
    {
        foreach(string bad_path in badEdge_path)
        {
            if (File.Exists(bad_path))
            {
                File.CreateText(bad_path).Dispose();
                File.Delete(bad_path);
            }
        }

        bad_edges = new List<string>[3];
    }

    //read the badEdge file after matlab done writing it
    public void read_badEdges()
    {
        List<string[]> badEdges = new List<string[]>();
        int i = 0; //flag to check badEdges list for all cases;

        foreach (string bad_path in badEdge_path)
        {
            if (!File.Exists(bad_path))
            {
                Debug.Log("something went wrong with matlab there is no badedge file available");
                continue;
            }
            else
            {
                try
                {
                    badEdges.Add(File.ReadAllLines(bad_path));

                    foreach (string badEdge in badEdges[i])
                    {
                        string[] bE_info = new string[2];
                        bE_info = badEdge.Split(',');

                        string badEdge_info = bE_info[0] + "_" + bE_info[1];

                        if (!bad_edges[i].Contains(badEdge_info))
                        {
                            bad_edges[i].Add(badEdge_info);
                        }
                    }

                    i++; //move on to next case;
                    Debug.Log("Done reading badEdge file");
                }
                catch (Exception e)
                {
                    Debug.Log("the file couldnt be read - " + e.Message);
                    continue;
                }
            }
        }
    }

    //get em blinking yo!! Are coroutines the best way to do this? I need to read up on these. 
    public void makeEm_blink(GameObject toggle)
    {
        //read_badEdges();
        on = toggle.GetComponent<Toggle>().isOn;
        if(bad_edges != null)
        {
            int this_case = 0;

            if (toggle.name.StartsWith("Profit"))
                this_case = 0;
            else if (toggle.name.StartsWith("Time"))
                this_case = 1;
            else
                this_case = 2;

            Manager.Instance.suggest[this_case] += 1;
            StartCoroutine(blinkers(this_case));
        }
    }

    public IEnumerator blinkers(int tog_no)
    {
        GameObject[] redLine = GameObject.FindGameObjectsWithTag("red");
        GameObject[] greenLine = GameObject.FindGameObjectsWithTag("green");
        GameObject[] blueLine = GameObject.FindGameObjectsWithTag("blue");

        //Queue<string> badEdges = new Queue<string>();
        while(on)
        {
            foreach (string bad_edge in bad_edges[tog_no])
            {
                update_bad(redLine, bad_edge, true);
                update_bad(greenLine, bad_edge, true);
                update_bad(blueLine, bad_edge, true);
            }

            yield return new WaitForSeconds(0.5f);

            foreach (string bad_edge in bad_edges[tog_no])
            {
                update_bad(redLine, bad_edge, false);
                update_bad(greenLine, bad_edge, false);
                update_bad(blueLine, bad_edge, false);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void update_bad(GameObject[] lines, string bad_edge, bool on)
    {
        Vector3 wobble = new Vector3(.3f, .3f, 0);

        foreach (GameObject line in lines)
        {
            Color lineColor = line.GetComponent<SpriteRenderer>().color;

            if (line.name.EndsWith("_" + bad_edge))
            {
                if(on)
                {
                    line.GetComponent<SpriteRenderer>().color = Color.yellow;
                    line.GetComponent<Transform>().localScale += wobble;
                }
                else
                {
                    line.GetComponent<SpriteRenderer>().color = lineColor;
                    line.GetComponent<Transform>().localScale -= wobble;
                }
            }
        }
    }
}