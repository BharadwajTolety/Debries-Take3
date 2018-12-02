using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User_ID : MonoBehaviour {

	public Text UserID;
	private int r1, r2,rs;
	private char n;
	string p = "Player", number, id;
	// Use this for initialization
    //Fucntion to create playerID
    void player_id()
	{
		r1 = Random.Range(1, 1000);
        r2 = Random.Range(1, 1000);
        rs = r1 + r2;
        //Debug.Log(p);
        number = rs.ToString();
        //Debug.Log(rs);
        id = string.Concat(p, number);
        //Debug.Log(id);
        UserID.text = id;
	}
	void Start () {
		player_id();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
