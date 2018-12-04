using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class User_ID : MonoBehaviour {

	public Text UserID;
	string writepath;
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
        Debug.Log(id);
        UserID.text = id;
	}
    void Writefile(string path)
	{
		StreamWriter swrite;
		if(!File.Exists(path))
		{
			swrite = File.CreateText(Application.dataPath + "/Write_Info.txt");
		}
		else
		{
			swrite = new StreamWriter(path);
		}
        swrite.WriteLine(id);
    
		swrite.Close();
	}
	void Start () {
		player_id();
		writepath = Application.dataPath + "/Write_Info.txt";
		Writefile(writepath);
        
	}

}
