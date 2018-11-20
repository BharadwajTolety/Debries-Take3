using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redCursor : MonoBehaviour {
  private Vector3 mousePosition;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 newSize = new Vector3 (Manager.Instance.brushSize, Manager.Instance.brushSize, 1);
      float moveSpeed = 1f;
        if (Manager.Instance.mySelection==1) {
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            


            
           if (this.tag == "txtSelected")
            {
                mousePosition.Set(mousePosition.x-250, mousePosition.y-250, mousePosition.z);
                transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
                //transform.localScale = 1.0f;
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
                transform.localScale = newSize;
            }
        }
	}

	void OnCollisionEnter2D(Collision2D other)
	{

		Debug.Log ("ok");

	}

}
