using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class map_transformations : MonoBehaviour {
    
    [SerializeField]
    private Vector3 rot = new Vector3(0, 0, -61), pos = new Vector3(-78.5f, 755.83f, 0), reset_pos = new Vector3(-10, 30, 0);
    [SerializeField]
    private float zoom = 0.05f;
    [SerializeField]
    private GameObject eye_mask;

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
        if(transform.localScale.x < 1.55f)
        {
            transform.localScale += new Vector3(zoom, zoom, zoom);
            transform.position += reset_pos;
        }
    }

    public void zoom_out()
    {
        if(transform.localScale.x > 0.95f)
        {
            transform.localScale -= new Vector3(zoom, zoom, zoom);
            transform.position -= reset_pos;
        }
    }

    public void edge_mask(GameObject eye)
    {
        Sprite[] open = { GameObject.FindGameObjectWithTag("redOpen").GetComponent<Image>().sprite,
                          GameObject.FindGameObjectWithTag("blueOpen").GetComponent<Image>().sprite,
                          GameObject.FindGameObjectWithTag("greenOpen").GetComponent<Image>().sprite
                        };

        Sprite[] close ={ GameObject.FindGameObjectWithTag("redClose").GetComponent<Image>().sprite,
                          GameObject.FindGameObjectWithTag("blueClose").GetComponent<Image>().sprite,
                          GameObject.FindGameObjectWithTag("greenClose").GetComponent<Image>().sprite
                        };

        switch (eye.name)
        {
            case "RedEye":      if(GameObject.Find("BlueEye").GetComponent<Image>().sprite == open[1])
                                {
                                    GameObject.Find("BlueEye").GetComponent<Image>().sprite = close[1];
                                    mask_edges("blueLine");
                                }
                                else if(GameObject.Find("GreenEye").GetComponent<Image>().sprite == open[2])
                                {
                                    GameObject.Find("GreenEye").GetComponent<Image>().sprite = close[2];
                                    mask_edges("greenLine");
                                }
                                    
                                if ( eye.GetComponent<Image>().sprite != open[0])
                                {
                                    eye.GetComponent<Image>().sprite = open[0];
                                }
                                else
                                {
                                    eye.GetComponent<Image>().sprite = close[0];
                                }

                                mask_edges("redLine");

                                break;

            case "BlueEye":     if(GameObject.Find("RedEye").GetComponent<Image>().sprite == open[0])
                                {
                                    GameObject.Find("RedEye").GetComponent<Image>().sprite = close[0];
                                    mask_edges("redLine");
                                }
                                else if(GameObject.Find("GreenEye").GetComponent<Image>().sprite == open[2])
                                {
                                    GameObject.Find("GreenEye").GetComponent<Image>().sprite = close[2];
                                    mask_edges("greenLine");
                                }
                                    
                                if (eye.GetComponent<Image>().sprite != open[1])
                                {
                                    eye.GetComponent<Image>().sprite = open[1];
                                }
                                else
                                    eye.GetComponent<Image>().sprite = close[1];

                                mask_edges("blueLine");

                                break;

            case "GreenEye":    if(GameObject.Find("BlueEye").GetComponent<Image>().sprite == open[1])
                                {
                                    GameObject.Find("BlueEye").GetComponent<Image>().sprite = close[1];
                                    mask_edges("blueLine");
                                }   
                                else if(GameObject.Find("RedEye").GetComponent<Image>().sprite == open[0])
                                {
                                    GameObject.Find("RedEye").GetComponent<Image>().sprite = close[0];
                                    mask_edges("redLine");
                                }
                                    
                                if (eye.GetComponent<Image>().sprite != open[2])
                                {
                                    eye.GetComponent<Image>().sprite = open[2];
                                }
                                else
                                    eye.GetComponent<Image>().sprite = close[2];

                                mask_edges("greenLine");
                                
                                break;

            default:            Debug.Log(eye.name);
                                Debug.Log(" nothing to mask here");
                                break;
        }
    }

    private void mask_edges(string edg)
    {
        string[] what_edge  = {"redLine" , "blueLine" , "greenLine"};

        for( int i = 0; i<what_edge.Length; i++)
        {   
            if(edg != what_edge[i])
            {
                GameObject[] edges = GameObject.FindGameObjectsWithTag(what_edge[i]);
                foreach (GameObject edge in edges)
                {
                    if (edge.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.None)
                    {
                        edge.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                        edge.GetComponent<BoxCollider2D>().isTrigger = false;
                    }
                    else
                    {
                        edge.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                        edge.GetComponent<BoxCollider2D>().isTrigger = true;
                    }
                }
            }
        }
    }
}
