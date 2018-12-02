using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_brush : MonoBehaviour {

	public void setBrushRed()
    {
        Manager.Instance.mySelection = 1;
        GameObject.FindGameObjectWithTag("blueCur").transform.position = new Vector3(-10000, 0, 0);
        //GameObject.FindGameObjectWithTag ("redCur").transform.position = new Vector3 (0, 0, 0);
        GameObject.FindGameObjectWithTag("greenCur").transform.position = new Vector3(-10000, 0, 0);
        GameObject.FindGameObjectWithTag("whiteCur").transform.position = new Vector3(-10000, 0, 0);
    }
   
}
