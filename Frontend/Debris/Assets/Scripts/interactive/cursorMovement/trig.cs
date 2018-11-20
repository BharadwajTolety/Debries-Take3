using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class trig : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		//Debug.Log (this.name );
		if (this.name == "cursorRed") {
			if (Input.GetMouseButton (0) == true && col.name != "cursorRed" && col.tag != "redLine") {
               //print(col.tag);
				AsignLine ("LineRed", col.name);
			}
		}
		else if(this.name == "cursorGreen") {
			if (Input.GetMouseButton (0) == true && col.name != "cursorGreen" && col.tag != "greenLine") {
				AsignLine ("LineGreen", col.name);
			}
		}
		else if(this.name == "cursorBlue") {
			if (Input.GetMouseButton (0) == true && col.name != "cursorBlue" && col.tag != "blueLine") {
				AsignLine ("LineBlue", col.name);
			}
		}

        else if (this.name == "cursorWhite")
        {
            if (Input.GetMouseButton(0) == true && col.name != "cursorWhite" && col.tag != "LineWhite")
            {
                AsignLine("LineWhite", col.name);
            }
        }


    }

	void AsignLine(string lineType,string lineName){

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
		created.transform.parent =GameObject.Find("Container").gameObject.transform ;
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
        created.transform.parent = GameObject.Find("Container").gameObject.transform;
        created.transform.localScale = distance;

        created.name = lineName;
    }










}
