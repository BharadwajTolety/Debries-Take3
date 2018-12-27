using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class undo_redo : MonoBehaviour
{
    int undo_count = 0; 

    //reset the move count
    private void reset_count_moves()
    {
        Manager.Instance.map_version = 0;
    }

    //redo button call
    public void redo()
    {
        if (Manager.Instance.map_version < undo_count && Manager.Instance.map_info.Count>1)
        {
            Manager.Instance.map_version++;
            map_update(Manager.Instance.map_info[Manager.Instance.map_version]);
        }
        else if(Manager.Instance.map_info.Count > 1)
        {
            undo_count = 0;
        }
    }

    //undo button call 
    public void undo()
    {
        if (Manager.Instance.map_version > 0)
        {
            Manager.Instance.map_version--;
            map_update(Manager.Instance.map_info[Manager.Instance.map_version]);
            undo_count++;
        }
    }

    //call this to update the map contractors/coloring
    private void map_update(Dictionary<string, string> updateEdges)
    {
        foreach(var updateEdge in updateEdges)
        {
            GameObject NewObj = GameObject.Find(updateEdge.Value);
            GameObject theSelectedObj = GameObject.Find(updateEdge.Key);

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

            created.name = updateEdge.Key;
        }
        Debug.Log("Map Updated");
    }
}