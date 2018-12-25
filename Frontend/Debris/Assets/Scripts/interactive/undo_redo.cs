using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class undo_redo : MonoBehaviour
{
    //reset the move count
    private void reset_count_moves()
    {
        Manager.Instance.count_move = 0;
    }

    //redo button call
    public void redo()
    {

    }

    //undo button call 
    public void undo()
    {

    }

    //get them edges for updating then call map update 
    private void get_edges(int map_ver)
    {
        Dictionary<int, string> map_inf = Manager.Instance.map_info[map_ver];
        GameObject[] redEdges = GameObject.FindGameObjectsWithTag("redLine");
        GameObject[] greenEdges = GameObject.FindGameObjectsWithTag("greenLine");
        GameObject[] blueEdges = GameObject.FindGameObjectsWithTag("blueLine");
        Dictionary<string, string> updateEdges = new Dictionary<string, string>();

        string[] nodeInfo = new string[4];

        for (int i = 0; i < redEdges.Length; i++)
        {
            nodeInfo = redEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                foreach (var keyVal in map_inf)
                {
                    if (keyVal.Value == (nodeInfo[2] + nodeInfo[3]))
                    {
                        updateEdges.Add("LineRed",redEdges[i].name);
                    }
                }
            }
        }

        for (int i = 0; i < greenEdges.Length; i++)
        {
            nodeInfo = greenEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                foreach (var keyVal in map_inf)
                {
                    if (keyVal.Value == (nodeInfo[2] + nodeInfo[3]))
                    {
                        updateEdges.Add("LineGreen", greenEdges[i].name);
                    }
                }
            }
        }

        for (int i = 0; i < blueEdges.Length; i++)
        {
            nodeInfo = blueEdges[i].name.Split('_');
            if (nodeInfo.Length > 2)
            {
                foreach (var keyVal in map_inf)
                {
                    if (keyVal.Value == (nodeInfo[2] + nodeInfo[3]))
                    {
                        updateEdges.Add("LineBlue", blueEdges[i].name);
                    }
                }
            }
        }

        map_update(updateEdges);
    }

    //call this to update the map contractors/coloring
    private void map_update(Dictionary<string, string> updateEdges)
    {
        foreach(var updateEdge in updateEdges)
        {
            GameObject NewObj = GameObject.Find(updateEdge.Key);
            GameObject theSelectedObj = GameObject.Find(updateEdge.Value);

            Vector3 distance = theSelectedObj.transform.localScale;

            Vector3 objScale = theSelectedObj.transform.localScale;
            Vector3 between2 = theSelectedObj.transform.position;
            Quaternion tetha = theSelectedObj.transform.rotation;

            // make sure you are deleting a line
            string objectName = theSelectedObj.name;

            if (objectName.Contains("E_"))
            {
                //print(objectName);
                Destroy(theSelectedObj);
            }

            GameObject created = Instantiate(NewObj, between2, Quaternion.identity);
            created.transform.rotation = tetha;//Rotate (startPoint, tetha);
            created.transform.parent = GameObject.Find("MapScreen").gameObject.transform;
            created.transform.localScale = distance;

            created.name = updateEdge.Value;
        }
    }
}