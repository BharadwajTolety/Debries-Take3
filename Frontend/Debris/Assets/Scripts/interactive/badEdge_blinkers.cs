using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class badEdge_blinkers : MonoBehaviour {

    private List<string> bad_edges = new List<string>();
    private bool on = true;
    private string badEdge_path;

    private void Awake()
    {
        badEdge_path = Application.streamingAssetsPath + "/Database/Input/badEdges_from_Matlab.csv";
        reset_badEdge();
    }

    //delete badEdge file so that matlab can rewrite it
    private void reset_badEdge()
    {
        if (File.Exists(badEdge_path))
        {
            File.CreateText(badEdge_path).Dispose();
            File.Delete(badEdge_path);
        }

        bad_edges = new List<string>();
    }

    //read the badEdge file after matlab done writing it
    public bool read_badEdges()
    {
        if (!File.Exists(badEdge_path))
        {
            Debug.Log("something went wrong with matlab there is no badedge file available");
            return false;
        }
        else
        {
            try
            {
                string[] badEdges = File.ReadAllLines(badEdge_path);

                foreach(string badEdge in badEdges)
                {
                    string[] bE_info = new string[2];
                    bE_info = badEdge.Split(',');

                    string badEdge_info = bE_info[0] + "_" + bE_info[1];

                    if (!bad_edges.Contains(badEdge_info))
                    {
                        bad_edges.Add(badEdge_info);
                    }
                }

                Debug.Log("Done reading badEdge file");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("the file couldnt be read - " + e.Message); 
                return false;
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
            Manager.Instance.suggest += 1;
            StartCoroutine(blinkers());
        }
    }

    public IEnumerator blinkers()
    {
        GameObject[] redLine = GameObject.FindGameObjectsWithTag("red");
        GameObject[] greenLine = GameObject.FindGameObjectsWithTag("green");
        GameObject[] blueLine = GameObject.FindGameObjectsWithTag("blue");

        //Queue<string> badEdges = new Queue<string>();
        while(on)
        {
            foreach (string bad_edge in bad_edges)
            {
                update_bad(redLine, bad_edge, true);
                update_bad(greenLine, bad_edge, true);
                update_bad(blueLine, bad_edge, true);
            }

            yield return new WaitForSeconds(0.5f);

            foreach (string bad_edge in bad_edges)
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