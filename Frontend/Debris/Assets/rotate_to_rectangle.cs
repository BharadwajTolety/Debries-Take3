using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_to_rectangle : MonoBehaviour {

    public Vector3 rot = new Vector3(0, 0, -61), pos = new Vector3(200, 500, 0);

    private void Start()
    {
       transform.Rotate(rot);
       transform.position = pos;
    }
}
