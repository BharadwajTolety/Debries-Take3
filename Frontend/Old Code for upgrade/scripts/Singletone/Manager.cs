﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager> {
    protected Manager(){
    }
  public int mySelection = 1;
  public float brushSize = 4000;
    public List<float> debrisList;
    public List<float>TimesList;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
