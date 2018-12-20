using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class trig : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D col)
 	{
		//Debug.Log (this.name );
		if (this.tag == "redCursor") {
            //Debug.Log(col.tag + " "  + this.tag);
            if (Input.GetMouseButton (0) == true && this.tag == "redCursor" && col.tag != "redLine") {
                Debug.Log("works red");
				AssignLine ("LineRed", col.name);
			}
		}
		else if(this.tag == "greenCursor") {
			if (Input.GetMouseButton (0) == true && this.tag == "greenCursor" && col.tag != "greenLine") {
                Debug.Log("works");
                AssignLine ("LineGreen", col.name);
			}
		}
		else if(this.tag == "blueCursor") {
			if (Input.GetMouseButton (0) == true && this.tag == "blueCursor" && col.tag != "blueLine") {
                Debug.Log("works");
                AssignLine ("LineBlue", col.name);
			}
		}
        else if (this.tag == "whiteCursor")
        {
            if (Input.GetMouseButton(0) == true && this.tag == "whiteCursor" && col.tag != "whiteLine")
            {
                AssignLine("LineWhite", col.name);
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
