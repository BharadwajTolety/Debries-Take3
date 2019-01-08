using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class trig : MonoBehaviour {

    //not the best place to do this but whatevs, this works out well.
    private GameObject scan;
    private GameObject blinker;
    private GameObject scan_off;
    private GameObject scan_on;

    //checking scans 
    int scanned;

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
        //Debug.Log(this.tag + " " + col.tag);
		if (col.tag == "redCursor" || col.tag == "greenCursor" || col.tag == "blueCursor" || col.tag == "whiteCursor")
        {
            if (Input.GetMouseButton (0) == true && this.tag != "redLine" && col.tag == "redCursor") {
                //Debug.Log("works red");
				AssignLine ("LineRed", this.name);
                Manager.Instance.flag = true;
            }
            else if (Input.GetMouseButton(0) == true && this.tag != "greenLine" && col.tag == "greenCursor")
            {
                //Debug.Log("works green");
                AssignLine("LineGreen", this.name);
                Manager.Instance.flag = true;
            }
            else if (Input.GetMouseButton(0) == true && this.tag != "blueLine" && col.tag == "blueCursor")
            {
                //Debug.Log("works blue");
                AssignLine("LineBlue", this.name);
                Manager.Instance.flag = true;
            }
            else if (Input.GetMouseButton(0) == true && this.tag != "whiteLine" && col.tag == "whiteCursor")
            {
                AssignLine("LineWhite", this.name);
                Manager.Instance.flag = true;
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
        }
    }

    void AssignLine(string lineType,string lineName){

		//Debug.Log (lineType);

		GameObject theSelectedObj;
		GameObject NewObj=GameObject.Find(lineType);
		theSelectedObj = GameObject.Find(lineName);

		Vector3 distance = theSelectedObj.transform.localScale;
			

		Vector3 objScale = theSelectedObj.transform.localScale;
		Vector3 between2 = theSelectedObj.transform.position;
		Quaternion tetha = theSelectedObj.transform.rotation;


        // make sure you are deleting a line
        string objectName = theSelectedObj.name;
        
        if (objectName.Contains("E_")) {
            //print(objectName);
            Destroy(theSelectedObj);
        }

		GameObject created= Instantiate (NewObj, between2, Quaternion.identity);
		created.transform.rotation=tetha;//Rotate (startPoint, tetha);
		created.transform.parent =GameObject.Find("MapScreen").gameObject.transform ;
		created.transform.localScale = distance;

		created.name = lineName;

        Manager.Instance.save_map(Manager.Instance.map_version,theSelectedObj);
        Manager.Instance.save_map(Manager.Instance.map_version + 1, created);

        Manager.Instance.edge_changes += 1;
            
    }

    private void deborah_check()
    {
        GameObject[] themWhiteLines = GameObject.FindGameObjectsWithTag("whiteLine");

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

    void AnimasignLine(string lineType, string lineName)
    {

        //Debug.Log (lineType);

        GameObject theSelectedObj;
        GameObject NewObj = GameObject.Find(lineType);
        theSelectedObj = GameObject.Find(lineName);

        Vector3 distance = theSelectedObj.transform.localScale;


        Vector3 objScale = theSelectedObj.transform.localScale;
        Vector3 between2 = theSelectedObj.transform.position;
        Quaternion tetha = theSelectedObj.transform.rotation;
        Destroy(theSelectedObj);


        GameObject created = Instantiate(NewObj, between2, Quaternion.identity);
        created.transform.rotation = tetha;//Rotate (startPoint, tetha);
        created.transform.parent = GameObject.Find("MapScreen").gameObject.transform;
        created.transform.localScale = distance;

        created.name = lineName;
    }
}
