using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class badEdge_blinkers : MonoBehaviour {

    private int[] bad_nodes = { 10,11,12,13,14,16,17,18,19};
    private bool on = true;

    public void makeEm_blink(GameObject toggle)
    {
        on = toggle.GetComponent<Toggle>().isOn;
        StartCoroutine(blinkers());
    }

    public IEnumerator blinkers()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("objNode");
        Vector3 wobble = new Vector3(.3f, .3f, 0);

        while(on)
        {
            int bad_node = 0;
            foreach (GameObject node in nodes)
            {
                if (node.name.Contains("n_" + bad_nodes[bad_node]))
                {
                    node.GetComponent<SpriteRenderer>().color = Color.yellow;
                    node.GetComponent<Transform>().localScale += wobble;
                    bad_node++;

                    if (bad_node >= bad_nodes.Length)
                        break;
                }
            }

            yield return new WaitForSeconds(.5f);
            bad_node = 0;

            foreach (GameObject node in nodes)
            {
                if (node.name.Contains("n_" + bad_nodes[bad_node]))
                {
                    node.GetComponent<SpriteRenderer>().color = Color.white;
                    node.GetComponent<Transform>().localScale -= wobble;
                    bad_node++;

                    if (bad_node >= bad_nodes.Length)
                        break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
