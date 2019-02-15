using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scan_controls : MonoBehaviour
{
    public Button profit_button, time_button, intersect_button;
    public Sprite[] profit_sprite, time_sprite, intersect_sprite;
    private int profit_obj, time_obj, intersect_obj, total;

    private void Awake()
    {
        profit_obj = 0;
        time_obj = 0;
        intersect_obj = 0;
        total = 0;
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

    public void scan_check()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<contInfo_Matlab>().read_contractor_info(profit_obj, time_obj, intersect_obj);
    }

    private void update_button(Button butt, Sprite[] spri, int pressed)
    {
        if (pressed == 1)
            butt.GetComponent<Image>().sprite = spri[0];
        else
            butt.GetComponent<Image>().sprite = spri[1];
    }
}
