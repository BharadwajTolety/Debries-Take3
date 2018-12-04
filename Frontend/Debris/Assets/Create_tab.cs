using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_tab : MonoBehaviour {

	public GameObject tab;
	public void create_new_tab()
	{
		Instantiate(tab,new Vector3(236,90,0), Quaternion.identity);
	}

}
