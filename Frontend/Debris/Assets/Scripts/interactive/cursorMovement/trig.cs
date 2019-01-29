using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class trig : mapBrushing {

    //not the best place to do this but whatevs, this works out well.
    private GameObject scan;
    private GameObject blinker;
    private GameObject scan_off;
    private GameObject scan_on;

    //checking scans and cursor size
    private int scanned;
    private float size_check;
    private bool cursorOff;

    private void Awake()
    {
        scanned = 0;

        scan = GameObject.Find("Scan");
        blinker = GameObject.Find("Toggle");
        scan_off = GameObject.Find("scan_off");
        scan_on = GameObject.Find("scan_on");
    }

    private void OnTriggerStay2D(Collider2D col)
 	{

        if (col.tag == "red" || col.tag == "green" || col.tag == "blue" || col.tag == "white" || col.tag.Contains("+"))
        {
            if (Input.GetMouseButton (0) == true && this.tag == "redCursor" && !col.tag.Contains("red")) {
                //Debug.Log("works red");
				AssignLine ("red", col.name);
                Manager.Instance.flag = true;
            }
            else if (Input.GetMouseButton(0) == true && this.tag == "greenCursor" && !col.tag.Contains("green"))
            {
                //Debug.Log("works green");
                AssignLine("green", col.name);
                Manager.Instance.flag = true;
            }
            else if (Input.GetMouseButton(0) == true && this.tag == "blueCursor" && !col.tag.Contains("blue"))
            {
                //Debug.Log("works blue");
                AssignLine("blue", col.name);
                Manager.Instance.flag = true;
            }
            else if (Input.GetMouseButton(0) == true && this.tag == "whiteCursor" && col.tag != "white")
            {
                AssignLine("white", col.name);
                Manager.Instance.flag = true;
            }
        }
        else if(col.tag == "AllColor")
        {
            if (Input.GetMouseButton(0) == true && this.tag == "whiteCursor" && col.tag != "white")
            {
                AssignLine("white", col.name);
                Manager.Instance.flag = true;
            }
        }
        else
        {
            if (col.name == "no Cursor")
            {
                if (!cursorOff)
                {
                    cursorOff = true;
                    size_check = Manager.Instance.brushSize;
                    Manager.Instance.brushSize = 4;
                }
            }
            else if(col.name == "on Cursor")
            {
                if (cursorOff)
                {
                    Manager.Instance.brushSize = size_check;
                    cursorOff = false;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && Manager.Instance.flag)
        {
            //Debug.Log("update " + Manager.Instance.flag);

            Manager.Instance.map_version += 1;
            Manager.Instance.flag = false;

            deborah_check();
        }

        if(scanned != Manager.Instance.scans)
        {
            scanned = Manager.Instance.scans;
            deborah_check();
            GameObject.Find("scanning_notification").GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void deborah_check()
    {
        GameObject[] themWhiteLines = GameObject.FindGameObjectsWithTag("white");

        if (Manager.Instance.edge_changes > 5 || themWhiteLines.Length > 1)
        {
            blinker.GetComponent<Toggle>().interactable = false;
        }
        else
        {
            blinker.GetComponent<Toggle>().interactable = true;
        }

        if (Manager.Instance.edge_changes < 2 || themWhiteLines.Length > 1)
        {
            scan.GetComponent<Button>().interactable = false;
        }
        else
        {
            scan.GetComponent<Button>().interactable = true;
        }
    }
}
