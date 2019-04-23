using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scan_controls : MonoBehaviour
{
    public Button profit_button, time_button, intersect_button;
    public Sprite[] profit_sprite, time_sprite, intersect_sprite;
    private int profit_obj, time_obj, intersect_obj, total;

    public GameObject scan, mapscreen, manager;
    public GameObject[] blinkers = new GameObject[3];

    public int wid, hght, w, h;
    public int x, y;

    //checking scans and cursor size
    private int scanned;
    private float playTime;

    private void Awake()
    {
        scanned = 0;

        playTime = 0;

        profit_obj = 0;
        time_obj = 0;
        intersect_obj = 0;
        total = 0;
    }

    private void Update()
    {
        //track playtime
        playTime += Time.deltaTime;

        if (Input.GetMouseButtonUp(0) && Manager.Instance.flag)
        {
            //Debug.Log("update " + Manager.Instance.flag);

            Manager.Instance.map_version += 1;
            Manager.Instance.flag = false;

            deborah_check();
        }

        if (scanned != Manager.Instance.scans)
        {
            scanned = Manager.Instance.scans;
            deborah_check();
        }
    }

    public void check_deborah()
    {
        //manual call to check when clicking scan objects after coloring edges
        deborah_check();
    }

    private void deborah_check()
    {
        GameObject[] themWhiteLines = GameObject.FindGameObjectsWithTag("white");

        if(profit_obj == 1 || time_obj == 1 || intersect_obj ==1)
        {
            if (Manager.Instance.edge_changes > 5 || themWhiteLines.Length > 1 && Manager.Instance.scans < 1)
            {
                foreach (GameObject blinker in blinkers)
                    blinker.GetComponent<Toggle>().interactable = false;
            }
            else
            {
                foreach (GameObject blinker in blinkers)
                    blinker.GetComponent<Toggle>().interactable = true;
            }

            if (Manager.Instance.edge_changes < 2 || (themWhiteLines.Length > 1 && Manager.Instance.scans < 1))
            {
                scan.GetComponent<Button>().interactable = false;
            }
            else if (scan.GetComponent<Button>().interactable == false)
            {
                //only update when scan is active cos only after clicking scan is the playtime logged
                Manager.Instance.time_played = playTime;
                scan.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void profit_check()
    {
        if (profit_obj == 1)
        {
            total--;
            profit_obj = 0;
        }
        else if (total < 2)
        {
            total++;
            profit_obj = 1;
        }

        update_button(profit_button, profit_sprite, profit_obj);
    }

    public void time_check()
    {
        if (time_obj == 1)
        {
            total--;
            time_obj = 0;
        }
        else if (total < 2)
        {
            total++;
            time_obj = 1;
        }

        update_button(time_button, time_sprite, time_obj);
    }

    public void intersect_check()
    {
        if (intersect_obj == 1)
        {
            total--;
            intersect_obj = 0;
        }
        else if(total < 2)
        {
            total++;
            intersect_obj = 1;
        }

        update_button(intersect_button, intersect_sprite, intersect_obj);
    }

    //public method to call scanning functionality
    public void scan_check()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<contInfo_Matlab>().read_contractor_info(profit_obj, time_obj, intersect_obj);
    }

    public void submit_log()
    {
        string player, session;
        if (Manager.Instance.sessionId == "" || Manager.Instance.playerId == "")
        {
            player = "def";
            session = "def";
        }
        else
        {
            player = Manager.Instance.playerId;
            session = Manager.Instance.sessionId;
        }
        string exPath = Application.streamingAssetsPath + "/Database/Output/" + player + "_" + session + "/Scan_Final.csv";
        GameObject.FindGameObjectWithTag("GameController").GetComponent<contInfo_Matlab>().write_log(exPath, profit_obj, time_obj, intersect_obj);

        new_run();
    }

    private void new_run()
    {
        Manager.Instance.run += 1;
        mapscreen.GetComponent<Map_Initiation>().drawMap_again();

        runSetup.takeScreenShot_static(wid,hght,x,y, w, h);
    }

    private void update_button(Button butt, Sprite[] spri, int pressed)
    {
        if (pressed == 1)
            butt.GetComponent<Image>().sprite = spri[0];
        else
            butt.GetComponent<Image>().sprite = spri[1];
    }
}
