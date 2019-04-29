using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class scan_controls : MonoBehaviour
{
    public Button profit_button, time_button, intersect_button;
    public Sprite[] profit_sprite, time_sprite, intersect_sprite;
    private int profit_obj, time_obj, intersect_obj, total;

    public Dropdown which_run;
    public GameObject scan, mapscreen, verControl;
    public GameObject[] blinkers = new GameObject[3];

    [Header("screenshot setting")]
    public int wid;
    public int hght, w, h, x, y;

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

        update_runList();
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

        //hacky way to make sure order of things for new run
        if (runSetup.screenshot())
        {
            runSetup.screenshot_done(false);
            mapscreen.GetComponent<Map_Initiation>().drawMap_again();
            verControl.GetComponent<ver_control>().empty_folder(true);
        }
    }

    private void update_runList()
    {
        string dir = Application.streamingAssetsPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId;

        if (!Directory.Exists(dir))
        {
            Debug.Log("no folder found to keep current run logs in");
            return;
        }
        else
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(dir);

            //update the run list drop down box
            which_run.ClearOptions();
            List<string> dropOptions = new List<string>();
            dropOptions.Add("Current");
            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Name.EndsWith(".csv"))
                {
                    dropOptions.Add(file.Name.Substring(0,file.Name.IndexOf(".csv")));
                }
            }
            which_run.AddOptions(dropOptions);

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
            if (Manager.Instance.edge_changes > 40 || themWhiteLines.Length > 1 && Manager.Instance.scans < 1)
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

    //submit the current run and log it down.
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
        string exPath = Application.streamingAssetsPath + "/Database/Output/" + player + "_" + session + "/Run_" + Manager.Instance.run + ".csv";
        runSetup.log_data(exPath, profit_obj, time_obj, intersect_obj);

        new_run();
    }

    public void finalise_run()
    {
        //checkpath and print fota
        string path = Application.streamingAssetsPath + "/Database/Output/" + Manager.Instance.playerId + "_" + Manager.Instance.sessionId + "/";
        if (which_run.options[which_run.value].text == "Current")
        {
            path += Manager.Instance.run.ToString() + ".csv";
        }
        else
        {
            path += which_run.options[which_run.value].text + ".csv";
        }
        string newpath = path.Substring(0, path.IndexOf(".csv")) + "_Final.csv";

        //change name of the finalise run file to mark it as finalise then quit game.
        File.Move(path, newpath);
        File.Delete(path);

        SceneManager.LoadScene("HomeMenu");
    }

    //new run DO: redraw map;restart graphs; scan = 0; vercontrl reset; 
    private void new_run()
    {
        runSetup.takeScreenShot_static(wid, hght, x, y, w, h);

        //two steps in update hacky fix
        //mapscreen.GetComponent<Map_Initiation>().drawMap_again();
        //verControl.GetComponent<ver_control>().empty_folder(true);

        update_runList();

        //new run; scan back to zero
        Manager.Instance.run += 1;
        Manager.Instance.scans = 0;
    }

    private void update_button(Button butt, Sprite[] spri, int pressed)
    {
        if (pressed == 1)
            butt.GetComponent<Image>().sprite = spri[0];
        else
            butt.GetComponent<Image>().sprite = spri[1];
    }
}
