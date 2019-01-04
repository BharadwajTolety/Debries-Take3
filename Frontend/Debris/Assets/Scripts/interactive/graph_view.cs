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


    private void Awake()
    {
        profit_view = GameObject.Find("profit_charts");
        time_view = GameObject.Find("time_charts");
        mapscreen = GameObject.Find("MapScreen");
        intersect_view = GameObject.Find("intersect_view");

        GameObject.Find("notification_whiteLines").SetActive(false);
        scan_disable();

        profit_view.SetActive(false);
        time_view.SetActive(false);
    }

    public void toggle_view(bool profit)
    {
        if (profit)
            if (!profit_view.activeSelf)
            {
                profit_view.SetActive(true);
                time_view.SetActive(false);
                mapscreen.SetActive(false);
            }
            else
            {
                profit_view.SetActive(false);
                time_view.SetActive(false);
                mapscreen.SetActive(true);
            }
        else
            if (!time_view.activeSelf)
            {
                profit_view.SetActive(false);
                time_view.SetActive(true);
                mapscreen.SetActive(false);
            }
            else
            {
                profit_view.SetActive(false);
                time_view.SetActive(false);
                mapscreen.SetActive(true);
            }
    }

    public void toggle_intersect()
    {
        GameObject red_text = GameObject.Find("red_text");
        GameObject blue_text = GameObject.Find("blue_text");
        GameObject green_text = GameObject.Find("green_text");

        if (!intersect_view.activeSelf)
        {
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

            intersect_view.SetActive(true);
            mapscreen.SetActive(false);
        }
        else
        {
            intersect_view.SetActive(false);
            mapscreen.SetActive(true);
        }
    }

    private void count_nodes()
    {

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
        scan.GetComponent<Image>().sprite = scan_off.GetComponent<SpriteRenderer>().sprite;
        blinker.GetComponent<Toggle>().interactable = false;
        blinker.GetComponentInChildren<Image>().color = blinker.GetComponent<Toggle>().colors.disabledColor;
    }
}
