using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomer : MonoBehaviour {


	private float zoom;
	void Update () {
	if(Input.GetAxis ("Mouse ScrollWheel")>0) {
			if (Camera.main.orthographicSize > 500) {
				Camera.main.orthographicSize -= 200;		} 
	}
	if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				if (Camera.main.orthographicSize <4000) {
					Camera.main.orthographicSize += 200;}}

		if (Input.GetMouseButton(0)) {
			//float sensitivity = 150;
		//	float x = Input.GetAxis("Mouse X");
		//	float y = Input.GetAxis("Mouse Y");
			//Camera.main.transform.Translate(new Vector3(-x, -y, 0) * sensitivity);
		//	Debug.Log (sensitivity);
		}



	}



}



