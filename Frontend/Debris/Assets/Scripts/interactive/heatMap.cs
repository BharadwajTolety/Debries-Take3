using UnityEngine;
using System.Collections;

public class heatMap : MonoBehaviour
{
    public void toggle_heatmap()
    {
        GameObject[] heats = GameObject.FindGameObjectsWithTag("heat");

        foreach(GameObject heat in heats)
        {
            if (heat.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.None)
            {
                heat.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
            else
            {
                heat.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            }
        }
    }
}
