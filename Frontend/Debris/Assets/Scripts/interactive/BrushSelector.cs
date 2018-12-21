using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using UnityEngine.EventSystems;
using System;


public class BrushSelector : MonoBehaviour {

    //move the other brushes out of the way and set the current brush ON!!

	public void setBrushRed(){
		Manager.Instance.mySelection = 1;
        
		GameObject.FindGameObjectWithTag ("blueCursor").transform.position = new Vector3 (10000, 0, 0);
		GameObject.FindGameObjectWithTag ("greenCursor").transform.position = new Vector3 (10000, 0, 0);
        GameObject.FindGameObjectWithTag("whiteCursor").transform.position = new Vector3(10000, 0, 0);
    }
	public void setBrushGreen()
    {
		Manager.Instance.mySelection = 2;

        GameObject.FindGameObjectWithTag("whiteCursor").transform.position = new Vector3(10000, 0, 0);
        GameObject.FindGameObjectWithTag ("blueCursor").transform.position = new Vector3 (10000, 0, 0);
        GameObject.FindGameObjectWithTag("redCursor").transform.position = new Vector3(10000, 0, 0);
	}
	public void setBrushBlue()
    {
		Manager.Instance.mySelection = 3;
        
        GameObject.FindGameObjectWithTag("whiteCursor").transform.position = new Vector3(10000, 0, 0);
        GameObject.FindGameObjectWithTag ("redCursor").transform.position = new Vector3 (10000, 0, 0);
		GameObject.FindGameObjectWithTag ("greenCursor").transform.position = new Vector3 (10000, 0, 0);
	}
    public void setBrushWhite()
    {
        Manager.Instance.mySelection = 4;

        GameObject.FindGameObjectWithTag("blueCursor").transform.position = new Vector3(10000, 0, 0);
        GameObject.FindGameObjectWithTag("redCursor").transform.position = new Vector3(10000, 0, 0);
        GameObject.FindGameObjectWithTag("greenCursor").transform.position = new Vector3(10000, 0, 0);
    }

    //every frame check for brush size
    private void Update()
	{
        brush_size();
	}

    //control size of brush with mouse wheel
    private void brush_size()
    {
        try
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Manager.Instance.brushSize > 10)
                {
                    Manager.Instance.brushSize -= 5;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Manager.Instance.brushSize < 50)
                {
                    Manager.Instance.brushSize += 5;
                }
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Null" + ex.Message);
        }
    }
}
