using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using UnityEngine.EventSystems;
using System;


public class BrushSelector : MonoBehaviour {

	public void setBrushRed(){
		Manager.Instance.mySelection = 1; 
		GameObject.FindGameObjectWithTag ("blueCur").transform.position = new Vector3 (-10000, 0, 0);
		//GameObject.FindGameObjectWithTag ("redCur").transform.position = new Vector3 (0, 0, 0);
		GameObject.FindGameObjectWithTag ("greenCur").transform.position = new Vector3 (-10000, 0, 0);
        GameObject.FindGameObjectWithTag("whiteCur").transform.position = new Vector3(-10000, 0, 0);
    }
	public void setBrushGreen(){
		Manager.Instance.mySelection = 2;
        GameObject.FindGameObjectWithTag("whiteCur").transform.position = new Vector3(-10000, 0, 0);
        GameObject.FindGameObjectWithTag ("blueCur").transform.position = new Vector3 (-10000, 0, 0);
		GameObject.FindGameObjectWithTag ("redCur").transform.position = new Vector3 (-10000, 0, 0);
	//	GameObject.FindGameObjectWithTag ("greenCur").transform.position = new Vector3 (0, 0, 0);
	}
	public void setBrushBlue(){
		Manager.Instance.mySelection = 3;
        //	GameObject.FindGameObjectWithTag ("blueCur").transform.position = new Vector3 (0, 0, 0);
        GameObject.FindGameObjectWithTag("whiteCur").transform.position = new Vector3(-10000, 0, 0);
        GameObject.FindGameObjectWithTag ("redCur").transform.position = new Vector3 (-10000, 0, 0);
		GameObject.FindGameObjectWithTag ("greenCur").transform.position = new Vector3 (-10000, 0, 0);
	}
    public void setBrushWhite()
    {
        Manager.Instance.mySelection = 4;
        GameObject.FindGameObjectWithTag("blueCur").transform.position = new Vector3(-10000, 0, 0);
        GameObject.FindGameObjectWithTag("redCur").transform.position = new Vector3(-10000, 0, 0);
        GameObject.FindGameObjectWithTag("greenCur").transform.position = new Vector3(-10000, 0, 0);
    }
    public void ChangeBrushSize(Slider slider)
	{
		
		try{
			Manager.Instance.brushSize = slider.value;
		}
		catch(NullReferenceException ex) {
			Debug.Log ("Null"+ ex.Message);
		}
		//Debug.Log("New wind direction: " + slider.value);
	}

}
