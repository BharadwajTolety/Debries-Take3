using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class graph_view : MonoBehaviour {

    GameObject profit_view;
    GameObject time_view;
    GameObject intersect_view;
    GameObject mapscreen;

    //the name doesn't make sense cause the functionality has been deprecated but it works on scan_disable stuff so it's still here.
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
}
