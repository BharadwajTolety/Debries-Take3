using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapBrushing : MonoBehaviour
{
    //call this for regular brushing map update
    public void AssignLine(string lineType, string lineName)
    {
        GameObject theSelectedObj = GameObject.Find(lineName);
        GameObject NewObj = GameObject.Find(lineType);

        if (theSelectedObj.tag.Contains("+"))
        {
            NewObj = GameObject.FindGameObjectWithTag("AllColor");
        }
        else if(theSelectedObj.tag == "white")
        {
            NewObj = GameObject.Find(lineType);
        }
        else if(Manager.Instance.scans > 0) //we don't want multple contractors on same edge before first scans
        {
            string update = NewObj.tag + theSelectedObj.tag;
            if (update.Contains("red") && update.Contains("blue"))
                update = "red+blue";
            else if (update.Contains("red") && update.Contains("green"))
                update = "red+green";
            else if (update.Contains("green") && update.Contains("blue"))
                update = "green+blue";
            else
                update = lineType;

            NewObj = GameObject.Find(update);
        }

        GameObject created = update_it(theSelectedObj, NewObj);
        created.name = lineName;

        Manager.Instance.save_map(Manager.Instance.map_version, theSelectedObj);
        Manager.Instance.save_map(Manager.Instance.map_version + 1, created);

        Manager.Instance.edge_changes += 1;

    }

    //call this to undo/redo update the map contractors/coloring
    public void map_update_undoRedo(Dictionary<string, string> updateEdges)
    {
        foreach (var updateEdge in updateEdges)
        {
            GameObject NewObj = GameObject.Find(updateEdge.Value);
            GameObject theSelectedObj = GameObject.Find(updateEdge.Key);

            GameObject created = update_it(theSelectedObj, NewObj);
            created.name = updateEdge.Key;
        }
        Debug.Log("Map Updated undo redo");
    }

    //call this to version update the map contractors/coloring
    public void map_update_ver(Dictionary<string, string> updateEdges)
    {
        GameObject[] allGameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));

        foreach (var updateEdge in updateEdges)
        {
            bool objSelected = false;
            string edgeName = updateEdge.Key;

            GameObject NewObj = GameObject.Find(updateEdge.Value);
            GameObject theSelectedObj = new GameObject();

            foreach (GameObject go in allGameObjects)
            {
                if (go.name.EndsWith(updateEdge.Key))
                {
                    theSelectedObj = go;
                    edgeName = go.name;
                    objSelected = true;
                    break;
                }
            }

            //if no objected not found GTFO
            if (!objSelected)
                break;

            GameObject created = update_it(theSelectedObj, NewObj);

            created.name = edgeName;
        }
        Debug.Log("Map Updated version control");
    }

    //for new run edge update
    public void new_run_update(GameObject selectedObj, GameObject newObj)
    {
        GameObject new_run_edge = update_it(selectedObj, newObj);
        new_run_edge.name = selectedObj.name;

        Debug.Log("new run starting, run no.: " + Manager.Instance.run);
    }

    private GameObject update_it(GameObject theSelectedObj, GameObject NewObj)
    {

        Vector3 distance = theSelectedObj.transform.localScale;

        Vector3 objScale = theSelectedObj.transform.localScale;
        Vector3 between2 = theSelectedObj.transform.position;
        Quaternion tetha = theSelectedObj.transform.rotation;

        // make sure you are deleting a line
        string objectName = theSelectedObj.name;
        if (objectName.Contains("E_"))
        {
            Destroy(theSelectedObj);
        }

        GameObject created = Instantiate(NewObj, between2, Quaternion.identity);
        created.transform.rotation = tetha;//Rotate (startPoint, tetha);
        created.transform.parent = GameObject.Find("MapScreen").gameObject.transform;
        created.transform.localScale = distance;

        return created;
    }
}
