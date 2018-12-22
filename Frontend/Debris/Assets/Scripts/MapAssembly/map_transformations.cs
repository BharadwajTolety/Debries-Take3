using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class map_transformations : MonoBehaviour {
    
    [SerializeField]
    private Vector3 rot = new Vector3(0, 0, -61), pos = new Vector3(-78.5f, 755.83f, 0), reset_pos = new Vector3(-10, 30, 0);
    [SerializeField]
    private float zoom = 0.05f;

    private void Start()
    {
        rotate_to_rectangle();
    }

    //rotate the map to rectangle (better fix be if we changed actual pos in map initiation but for now this will do)
    private void rotate_to_rectangle()
    {
        transform.Rotate(rot);
        transform.position = pos;
    }

    //map zoom in
    public void zoom_in()
    {
        if(transform.localScale.x < 1.55f)
        {
            transform.localScale += new Vector3(zoom, zoom, zoom);
            transform.position += reset_pos;
        }
    }

    //map zoom out
    public void zoom_out()
    {
        if(transform.localScale.x > 0.95f)
        {
            transform.localScale -= new Vector3(zoom, zoom, zoom);
            transform.position -= reset_pos;
        }
    }

    //turn cnc eyes on and off
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

        string[] eyes = { "RedEye", "BlueEye", "GreenEye" };
        string[] edges = { "redLine", "blueLine" , "greenLine" };

        for(int i = 0 ; i < eyes.Length ; i++)
        {
            if(eyes[i] != eye.name)
            {
                if (GameObject.Find(eyes[i]).GetComponent<Image>().sprite == open[i])
                {
                    GameObject.Find(eyes[i]).GetComponent<Image>().sprite = close[i];
                    mask_edges(edges[i]);
                }
            }
            else
            {
                if (eye.GetComponent<Image>().sprite != open[i])
                {
                    eye.GetComponent<Image>().sprite = open[i];
                }
                else
                {
                    eye.GetComponent<Image>().sprite = close[i];
                }

                mask_edges(edges[i]);
            }
        }
    }

    //mask the other edges out based on eye buttons
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
