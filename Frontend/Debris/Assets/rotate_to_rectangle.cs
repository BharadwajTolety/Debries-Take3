using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_to_rectangle : MonoBehaviour {
    private void Start()
    {
       transform.Rotate(0,0,-61);
       transform.position = new Vector3(200, 500, 0);
    }
}
