using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    public int mySelection = 1;
    public float brushSize = 50;

    public List<float> debrisList;
    public List<float> TimesList;
}
