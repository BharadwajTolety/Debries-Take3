using UnityEngine;
using System.Collections;

public class heatMap : MonoBehaviour
{
    public GameObject legend;

    public void toggle_heatmap()
    {
        GameObject[] heats = GameObject.FindGameObjectsWithTag("heat");

        foreach(GameObject heat in heats)
        {
            if (heat.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.None)
            {
                heat.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                legend.SetActive(false);
            }
            else
            {
                heat.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                legend.SetActive(true);
            }
        }
    }
}
