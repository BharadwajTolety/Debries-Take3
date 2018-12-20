using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_transformations : MonoBehaviour {
    
    [SerializeField]
    private Vector3 rot = new Vector3(0, 0, -61), pos = new Vector3(-53, 803, 0);
    [SerializeField]
    private float zoom = 0.05f;

    private void Start()
    {
        rotate_to_rectangle();
    }

    private void rotate_to_rectangle()
    {
        transform.Rotate(rot);
        transform.position = pos;
    }

    public void zoom_in()
    {
        transform.localScale += new Vector3(zoom, zoom, zoom);
    }

    public void zoom_out()
    {
        transform.localScale -= new Vector3(zoom, zoom, zoom);
    }
}
