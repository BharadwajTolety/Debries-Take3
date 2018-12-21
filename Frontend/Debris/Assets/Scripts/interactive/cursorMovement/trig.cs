using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class trig : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D col)
 	{
        Debug.Log(this.tag + " " + col.tag);
		if (col.tag == "redCursor") {
            if (Input.GetMouseButton (0) == true && this.tag != "redLine" && col.tag == "redCursor") {
                Debug.Log("works red");
				AssignLine ("LineRed", this.name);
			}
		}
		else if(col.tag == "greenCursor") {
			if (Input.GetMouseButton (0) == true && this.tag != "greenLine" && col.tag == "greenCursor") {
                Debug.Log("works");
                AssignLine ("LineGreen", this.name);
			}
		}
		else if(col.tag == "blueCursor") {
			if (Input.GetMouseButton (0) == true && this.tag != "blueLine" && col.tag == "blueCursor") {
                Debug.Log("works");
                AssignLine ("LineBlue", this.name);
			}
		}
        else if (col.tag == "whiteCursor")
        {
            if (Input.GetMouseButton(0) == true && this.tag != "whiteLine" && col.tag == "whiteCursor")
            {
                AssignLine("LineWhite", this.name);
            }
        }
    }

	void AssignLine(string lineType,string lineName){

		Debug.Log (lineType);

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
