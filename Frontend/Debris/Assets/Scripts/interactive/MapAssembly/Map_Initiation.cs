using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;


/* 
 The application run in a procedural format 
 1. initiation puts the nodes on the screen
 2. --Input Given solutions is done using inputGivenSolution() 
 */

public class Map_Initiation : MonoBehaviour
{
    private string JSONstring, JSON_Edges, JSON_Contractors;
    private JsonData itemData, edgeNamesData, ContractorsData;
    public float unit_r;
    public float TransX;
    public float TransY;
    struct Edges
    {
        public Vector3 start;
        public Vector3 end;
        public int id;
        public Edges(Vector3 startPoint, Vector3 endPoint, int index)
        {
            start = startPoint;
            end = endPoint;
            id = index;
        }

    }

    void Awake()
    {
        //C1- read the data for the nodes and put them into itemdata
        //C2- Run DrawMap()
        JSONstring = File.ReadAllText(Application.dataPath + "/Database/Input/Node_data_1.json");

        itemData = JsonMapper.ToObject(JSONstring);

        unit_r = 80; // the scale between two nodes
        TransX = 30;
        TransY = -100;
        DrawMap();
    }


    /// <summary>
    /// This function adds a node to the positions pos
    /// </summary>
    void AddNode(Vector3 pos, string strTag, string name)
    {
        GameObject theSelectedObj;
        GameObject NewObj;

        theSelectedObj = GameObject.FindGameObjectWithTag(strTag);
        NewObj = Instantiate(theSelectedObj, pos, Quaternion.identity);
        NewObj.transform.SetParent(gameObject.transform, true);
        NewObj.name = name;
    }

    void AddNodeText(Vector3 pos, string strTag, string name, int code)
    {
        GameObject theSelectedObj;
        GameObject NewObj;
        theSelectedObj = GameObject.FindGameObjectWithTag("txtValue");
        NewObj = Instantiate(theSelectedObj, pos, Quaternion.identity);
        NewObj.transform.SetParent(gameObject.transform, true);
        NewObj.name = name;

        //NewObj.GetComponent<GUIText>().text = code.ToString();
        NewObj.GetComponent<TextMesh>().text = code.ToString();

        GameObject UItextGO = new GameObject("Text2");
        UItextGO.transform.SetParent(gameObject.transform, true);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(pos.x, pos.y);

        Text text = UItextGO.AddComponent<Text>();
        text.text = code.ToString();
        text.fontSize = 40;
        text.color = Color.black;

        //   return UItextGO;
    }

    /// <summary>
    /// Kills the obj by name.
    /// </summary>
    /// <param name="name">Name.</param>
    void KillObjbyName(string name)
    {
        GameObject[] list;
        list = GameObject.FindGameObjectsWithTag("objNode");
        if (list != null)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].name.Equals(name))
                {
                    Destroy(list[i]);
                }
            }
        }
    }


    /// <summary>
    /// Draws the map.
    /// </summary>
    /// 
    /// 
    void DrawMap()
    {
        List<Vector3> Nodes = new List<Vector3>();
        int number_of_nodes = itemData["NodeData"].Count;
        double Pxx = 0;
        double Pyy = 0;
        float Px = 0;
        float Py = 0;
        int edgeNumber = 0;

        for (int i = 0; i < number_of_nodes; i++)
        {
            Pxx = (double)itemData["NodeData"][i]["x"];
            Pyy = (double)itemData["NodeData"][i]["y"];
            Px = (float)(Pxx / 500) + 8;
            Py = (float)(Pyy / 500);
            Nodes.Add(new Vector3((float)Px * unit_r, (float)Py * unit_r, 0));
        }

        //	foreach(Vector3 pos in Nodes){
        int nodes_length = Nodes.Count;
        Vector3 pos;
        for (int i = 0; i < nodes_length; i++)
        {
            pos = Nodes[i];
            AddNode(pos, "objNode", "n_" + (i+1));
            //addNodeText (pos, "txtVal", "txt"+i,i+1);
        }

        int number_of_edges = itemData["EdgeData"].Count;
        int source;
        int dest;
        int contractorCode;
        float debrivalue;
        float timesValue;


        // here we assign the debris to singltone public variale
        Manager.Instance.debrisList = new List<float>();
        Manager.Instance.TimesList = new List<float>();

        for (int i = 0; i < number_of_edges; i++)
        {

            //Manager.Instance.debrisList.Add((float)itemData["EdgeData"][i]["debris"]);

            source = (int)itemData["EdgeData"][i]["From"];
            //contractorCode = (int)itemData ["CD"];
            dest = (int)itemData["EdgeData"][i]["To"];
            contractorCode = (int)itemData["EdgeData"][i]["Contractor"];
            //Debug.Log(dest);
            edgeNumber = i + 1;

            if (contractorCode == 0) { AddEdge(edgeNumber, source, dest, "LineWhite"); }
            else
            if (contractorCode == 1) { AddEdge(edgeNumber, source, dest, "LineRed"); }
            else
            if (contractorCode == 2) { AddEdge(edgeNumber, source, dest, "LineBlue"); }
            else
            if (contractorCode == 3) { AddEdge(edgeNumber, source, dest, "LineGreen"); }


            debrivalue = float.Parse(itemData["EdgeData"][i]["debris"].ToString());
            Manager.Instance.debrisList.Add(debrivalue);
            timesValue = float.Parse(itemData["EdgeData"][i]["time"].ToString());
            Manager.Instance.TimesList.Add(timesValue);
        }
    }

    void AddEdge(int EdgeNumber, int nFrom, int nTo, string strType)
    {
        GameObject theSourceObj, theDestinationObj;
        theSourceObj = GameObject.Find("n_" + nFrom);
        theDestinationObj = GameObject.Find("n_" + nTo);
        Vector3 startPoint, endPoint;

        if (theSourceObj != null && theDestinationObj != null) //make sure the start and end node are given
        {
            startPoint = new Vector3(theSourceObj.transform.position.x, theSourceObj.transform.position.y);
            endPoint = new Vector3(theDestinationObj.transform.position.x, theDestinationObj.transform.position.y);
            Vector3 between = (endPoint - startPoint);
            Vector3 between2 = Vector3.Lerp(startPoint, endPoint, .5f);

            //	addNode (new Vector3(between2.x,between2.y), "square", "s"+nFrom+"_"+nTo);

            float distance = between.magnitude;
            float tetha = 90 + Mathf.Atan((startPoint.y - endPoint.y) / (startPoint.x - endPoint.x)) * 180 / Mathf.PI;
            string name = "E_" + EdgeNumber + "_" + nFrom + "_" + nTo;
            //print (name);
            GameObject theSelectedObj;
            GameObject NewObj;
            theSelectedObj = GameObject.Find(strType);
            NewObj = Instantiate(theSelectedObj, between2, Quaternion.identity);
            NewObj.transform.localScale = new Vector3(0.3f, distance, 0.0f); // Parameter1: the scale relative to the parent
            NewObj.transform.Rotate(Vector3.forward * 1 * tetha);//Rotate (startPoint, tetha);
            NewObj.transform.parent = gameObject.transform;
            NewObj.name = name;

        }
        else { }
        //Debug.Log("Not Found");
    }
}
