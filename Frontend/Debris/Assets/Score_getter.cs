using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Score_getter : MonoBehaviour {

	public Text[] score_display; 
	string readpath;
	public string[] scores;
	int i = 0,j=1;

	void readfile(string filepath)
	{
		StreamReader sReader = new StreamReader(filepath);
		while(!sReader.EndOfStream)
		{
			while(i<5)
			{
				scores[i] = sReader.ReadLine();
				Debug.Log(scores[i]);
				i++;
			}
			i = 0;
		}
		while(i<5)
		{
			score_display[i].text = "Run " + j.ToString() + ": " + scores[i];
			j++;
			i++;
		}
		sReader.Close();
	}
 
	void Start ()
	{
		readpath = Application.dataPath + "/Barru.txt";
		readfile(readpath);
	}
 
}
