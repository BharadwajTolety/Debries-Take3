using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graph_view : MonoBehaviour {

    GameObject profit_view;
    GameObject time_view;
    GameObject mapscreen;

    private void Awake()
    {
        profit_view = GameObject.Find("profit_charts");
        time_view = GameObject.Find("time_charts");
        mapscreen = GameObject.Find("MapScreen");

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
}
