using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class graph_view : MonoBehaviour {

    GameObject profit_view;
    GameObject time_view;
    GameObject intersect_view;
    GameObject mapscreen;
    GameObject scan;

    bool profit_active, time_active, intersect_active;

    private void Awake()
    {
        profit_view = GameObject.Find("profit_charts");
        time_view = GameObject.Find("time_charts");
        mapscreen = GameObject.Find("MapScreen");
        intersect_view = GameObject.Find("intersect_view");

        GameObject.Find("notification_whiteLines").SetActive(false);
        profit_active = false;
        time_active = false;
        intersect_active = false;
        scan_disable();
    }

    public void toggle_view(bool profit)
    {
        if (profit)
            if (!profit_active)
            {
                profit_view.transform.position = new Vector3(0, 0, 0);
                intersect_view.transform.position = new Vector3(251, 1188, 0);
                time_view.transform.position = new Vector3(-2554, 0, 0);
                mapscreen.SetActive(false);

                profit_active = true;
                time_active = false;
                intersect_active = false;
            }
            else
            {
                profit_view.transform.position = new Vector3(1957, 0, 0);
                time_view.transform.position = new Vector3(-2554, 0, 0);
                intersect_view.transform.position = new Vector3(251, 1188, 0);
                mapscreen.SetActive(true);

                time_active = false;
                intersect_active = false;
                profit_active = false;
            }
        else
            if (!time_active)
            {
                profit_view.transform.position = new Vector3(1957, 0, 0);
                time_view.transform.position = new Vector3(0, 0, 0);
                intersect_view.transform.position = new Vector3(251, 1188, 0);
                mapscreen.SetActive(false);
                
                time_active = true;
                profit_active = false;
                intersect_active = false;
            }
            else
            {
                profit_view.transform.position = new Vector3(1957, 0, 0);
                time_view.transform.position = new Vector3(-2554, 0, 0);
                intersect_view.transform.position = new Vector3(251, 1188, 0);
                mapscreen.SetActive(true);
                
                time_active = false;
                intersect_active = false;
                profit_active = false;
            }
    }

    public void toggle_intersect()
    {
        if (!intersect_active)
        {
            intersect_active = true;
            time_active = false;
            profit_active = false;

            intersect_view.transform.position = new Vector3(251, 65, 0);
            profit_view.transform.position = new Vector3(1957, 0, 0);
            time_view.transform.position = new Vector3(-2554, 0, 0);

            GameObject red_text = GameObject.Find("red_text");
            GameObject blue_text = GameObject.Find("blue_text");
            GameObject green_text = GameObject.Find("green_text");

            GameObject[] themRedEdges = GameObject.FindGameObjectsWithTag("redLine");
            GameObject[] themBlueEdges = GameObject.FindGameObjectsWithTag("blueLine");
            GameObject[] themGreenEdges = GameObject.FindGameObjectsWithTag("greenLine");

            List<int> node = new List<int>();
            string[] nodeInfo = new string[4];

            for (int i = 0; i < themRedEdges.Length; i++)
            {
                nodeInfo = themRedEdges[i].name.Split('_');
                if (nodeInfo.Length > 2)
                {
                    if(!node.Contains(int.Parse(nodeInfo[2])))
                        node.Add(int.Parse(nodeInfo[2]));

                    if(!node.Contains(int.Parse(nodeInfo[3])))
                        node.Add(int.Parse(nodeInfo[3]));

                    red_text.GetComponent<Text>().text = node.Count.ToString();
                }
            }

            node.Clear();
            for (int i = 0; i < themGreenEdges.Length; i++)
            {
                nodeInfo = themGreenEdges[i].name.Split('_');
                if (nodeInfo.Length > 2)
                {
                    if (!node.Contains(int.Parse(nodeInfo[2])))
                        node.Add(int.Parse(nodeInfo[2]));

                    if (!node.Contains(int.Parse(nodeInfo[3])))
                        node.Add(int.Parse(nodeInfo[3]));

                    green_text.GetComponent<Text>().text = node.Count.ToString();
                }
            }

            node.Clear();
            for (int i = 0; i < themBlueEdges.Length; i++)
            {
                nodeInfo = themBlueEdges[i].name.Split('_');
                if (nodeInfo.Length > 2)
                {
                    if (!node.Contains(int.Parse(nodeInfo[2])))
                        node.Add(int.Parse(nodeInfo[2]));

                    if (!node.Contains(int.Parse(nodeInfo[3])))
                        node.Add(int.Parse(nodeInfo[3]));

                    blue_text.GetComponent<Text>().text = node.Count.ToString();
                }
            }

            mapscreen.SetActive(false);
        }
        else
        {
            profit_view.transform.position = new Vector3(1957, 0, 0);
            time_view.transform.position = new Vector3(-2554, 0, 0);
            intersect_view.transform.position = new Vector3(251, 1188, 0);
            mapscreen.SetActive(true);

            time_active = false;
            intersect_active = false;
            profit_active = false;
        }
    }

    public void toggle_noti(GameObject notification)
    {
        GameObject[] themWhiteLines = GameObject.FindGameObjectsWithTag("whiteLine");
        if(themWhiteLines.Length > 1)
        {
            notification.SetActive(true);
            scan_disable();
        }
    }

    private void scan_disable()
    {
        GameObject scan = GameObject.Find("Scan");
        GameObject blinker = GameObject.Find("Toggle");
        GameObject scan_off = GameObject.Find("scan_off");

        scan.GetComponent<Button>().interactable = false;
        blinker.GetComponent<Toggle>().interactable = false;
    }
}
