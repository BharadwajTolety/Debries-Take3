using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //get the selected edges and contractors
        OneContractorsOnly("redLine", "txtRed");
        OneContractorsOnly("blueLine", "txtBlue");
        OneContractorsOnly("greenLine", "txtGreen");
    }

    private int OneContractorsOnly(string objName,string txtName)
    {
        int num= GameObject.FindGameObjectsWithTag(objName).Length;
        GameObject.Find(txtName).GetComponent<UnityEngine.UI.Text>().text = num.ToString();
        return 0;
    }

}