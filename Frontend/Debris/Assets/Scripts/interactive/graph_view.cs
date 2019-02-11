using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class graph_view : MonoBehaviour {

    GameObject profit_view;
    GameObject time_view;
    GameObject intersect_view;
    GameObject mapscreen;

    private void Awake()
    {
        mapscreen = GameObject.Find("MapScreen");

        scan_disable();
    }

    public void toggle_noti()
    {
        GameObject[] themWhiteLines = GameObject.FindGameObjectsWithTag("whiteLine");
        if (themWhiteLines.Length > 1)
        {
            scan_disable();
        }
        else
        {
            GameObject.Find("Toggle").GetComponent<Toggle>().interactable = true;
        }
    }

    private void scan_disable()
    {
        GameObject scan = GameObject.Find("Scan");
        GameObject blinker = GameObject.Find("Toggle");

        scan.GetComponent<Button>().interactable = false;
        blinker.GetComponent<Toggle>().interactable = false;
    }

    public void update_log(int profit_obj, int time_obj, int intersect_obj)
    {
        string exPath = Application.dataPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId + "/Scan_" + Manager.Instance.scans + ".csv";
        GameObject.FindGameObjectWithTag("GameController").GetComponent<contInfo_Matlab>().write_log(exPath, profit_obj, time_obj, intersect_obj);

        update_graphs();
        update_verCont();
    }

    private void update_graphs()
    {
        GameObject.Find("current_time").GetComponent<BarChart>().update_graph();
        GameObject.Find("current_profit").GetComponent<BarChart>().update_graph();
        GameObject.Find("current_total").GetComponent<LineChart>().update_graph();
    }

    //just update version control scores every scan
    private void update_verCont()
    {
        GameObject.Find("ver_control").GetComponent<ver_control>().update_verList();
    }
}
