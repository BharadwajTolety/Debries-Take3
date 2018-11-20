using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteCursor : MonoBehaviour {
    private Vector3 mousePosition;
    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {

        Vector3 newSize = new Vector3(Manager.Instance.brushSize, Manager.Instance.brushSize, 1);

        float moveSpeed = 1f;
        if (Manager.Instance.mySelection == 4)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
            transform.localScale = newSize;
        }

    }

}
