using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class badEdge_blinkers : MonoBehaviour {

    private List<int> bad_nodes = new List<int>();
    private bool on = true;
    private string badEdge_path;

    private void Awake()
    {
        badEdge_path = Application.dataPath + "/Database/Input/badEdges_from_Matlab.csv";
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

        bad_nodes = new List<int>();
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

                    int[] badEdge_info = { int.Parse(bE_info[0]), int.Parse(bE_info[1]) };

                    if (!bad_nodes.Contains(badEdge_info[0]))
                        bad_nodes.Add(badEdge_info[0]);
                    if (!bad_nodes.Contains(badEdge_info[1]))
                        bad_nodes.Add(badEdge_info[1]);
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
        if(bad_nodes != null)
            StartCoroutine(blinkers());
    }

    public IEnumerator blinkers()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("objNode");
        Vector3 wobble = new Vector3(.3f, .3f, 0);

        while(on)
        {
           foreach (GameObject node in nodes)
            {
                foreach (int bad_node in bad_nodes)
                {
                    if (node.name.Contains("n_" + bad_node))
                    {
                        node.GetComponent<SpriteRenderer>().color = Color.yellow;
                        node.GetComponent<Transform>().localScale += wobble;
                        break;
                    }                   
                }
            }

            yield return new WaitForSeconds(.5f);
           
            foreach (GameObject node in nodes)
            {
                foreach (int bad_node in bad_nodes)
                {
                    if (node.name.Contains("n_" + bad_node))
                    {
                        node.GetComponent<SpriteRenderer>().color = Color.white;
                        node.GetComponent<Transform>().localScale -= wobble;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}