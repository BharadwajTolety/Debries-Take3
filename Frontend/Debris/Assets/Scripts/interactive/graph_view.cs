using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class graph_view : MonoBehaviour {

    GameObject intersect_view;
    GameObject mapscreen;

    public GameObject scan, error;

    private void Awake()
    {
        mapscreen = GameObject.Find("MapScreen");
        intersect_view = GameObject.Find("intersection_overlaps");

        scan_disable();
    }

    public void toggle_noti()
    {
        GameObject[] themWhiteLines = GameObject.FindGameObjectsWithTag("whiteLine");
        if (themWhiteLines.Length > 1 && Manager.Instance.scans< 1)
        {
            scan_disable();
        }
        else
        {
            scan.SetActive(true);
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

    //update the graphs and then call function to write up the log for experiment stuff
    public void update_log(bool scanning, int profit_obj, int time_obj, int intersect_obj)
    {
        if (scanning)
        {
            string exPath = Application.dataPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId + "/Scan_" + Manager.Instance.scans + ".csv";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<contInfo_Matlab>().write_log(exPath, profit_obj, time_obj, intersect_obj);

            update_graphs();
            update_verCont();
        }
        scan.SetActive(false);
    }

    //controller to open and close graphs
    public void expand_graph(GameObject graph)
    {
        Animator graph_anim = graph.GetComponent<Animator>();
        Animator map_anim = mapscreen.GetComponent<Animator>();

        map_anim.GetComponent<Animator>().enabled = true;

        if (graph.name.Contains("_time"))
        {
            GameObject.Find("current_profit").GetComponent<Animator>().SetInteger("profit_state", 1);
            GameObject.Find("current_total").GetComponent<Animator>().SetInteger("progress_state", 1);

            if (graph_anim.GetInteger("time_state") == 2)
            {
                graph_anim.SetInteger("time_state", 1);
                map_anim.SetInteger("map_state", 1);
            }
            else
            {
                graph_anim.SetInteger("time_state", 2);
                map_anim.SetInteger("map_state", 3);
            }
        }
        else if(graph.name.Contains("_profit"))
        {
            GameObject.Find("current_time").GetComponent<Animator>().SetInteger("time_state", 1);
            GameObject.Find("current_total").GetComponent<Animator>().SetInteger("progress_state", 1);

            if (graph_anim.GetInteger("profit_state") == 2)
            {
                graph_anim.SetInteger("profit_state", 1);
                map_anim.SetInteger("map_state", 1);
            }
            else
            {
                graph_anim.SetInteger("profit_state", 2);
                map_anim.SetInteger("map_state", 2);
            }
        }
        else if(graph.name.Contains("_total"))
        {
            GameObject.Find("current_profit").GetComponent<Animator>().SetInteger("profit_state", 1);
            GameObject.Find("current_time").GetComponent<Animator>().SetInteger("time_state", 1);

            if (graph_anim.GetInteger("progress_state") == 2)
            {
                graph_anim.SetInteger("progress_state", 1);
                map_anim.SetInteger("map_state", 1);
            }
            else
            {
                graph_anim.SetInteger("progress_state", 2);
                map_anim.SetInteger("map_state", 4);
            }
        }
    }

    private void intersection_update()
    {
        Text text = intersect_view.GetComponentInChildren<Text>();
        Slider intersect_slider = intersect_view.GetComponentInChildren<Slider>();

        if (text.name == "total nodes")
        {
            text.text = (GameObject.FindGameObjectsWithTag("objNode").Length*2).ToString();
        }

        intersect_slider.maxValue = GameObject.FindGameObjectsWithTag("objNode").Length*2;
        if (intersect_slider.GetComponentInChildren<Text>().name == "current_overlap")
        {
            intersect_slider.value = Manager.Instance.intersect;
            intersect_slider.GetComponentInChildren<Text>().text = intersect_slider.value.ToString();
        }
    }

    private void update_graphs()
    {
        GameObject.Find("current_time").GetComponent<BarChart>().update_graph();
        GameObject.Find("current_profit").GetComponent<BarChart>().update_graph();
        GameObject.Find("current_total").GetComponent<LineChart>().update_graph();

        intersection_update();
    }

    //just update version control scores every scan
    private void update_verCont()
    {
        GameObject.Find("ver_control").GetComponent<ver_control>().update_verList();
    }

    public void error_msg_open()
    {
        mapscreen.SetActive(false);
        error.SetActive(true);
    }

    public void error_confirm()
    {
        SceneManager.LoadScene("HomeMenu");
    }
}
