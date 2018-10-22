using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSwitch : MonoBehaviour {
	public Sprite OffSprite;
    public Sprite OnSprite;
    public B but;
    public void ChangeImage()
    {
        if (but.image.sprite == OnSprite)
            but.image.sprite = OffSprite;
        else
        {
            but.image.sprite = OnSprite;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
