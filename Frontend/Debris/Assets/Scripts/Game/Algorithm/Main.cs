using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour{

    public class Contractor
    {
        public int id;
        public List<int> Nodes;
        public List<int> Edges;
        public List<int> Clusters;

        public void Init()
        {
            Nodes = new List<int>();
            Edges = new List<int>();
            Clusters = new List<int>();
        }
    }

    private struct Edges
    {
        public int id;
        public int contractor;
        public int e1, e2;
    }

    private List<Edges> allEdges;
    public Contractor Con_Red = new Contractor();
    public Contractor Con_Blue = new Contractor();
    public Contractor Con_Green = new Contractor();

    public Contractor[] Con_All = new Contractor[3];


    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            Con_All[i] = new Contractor();
        }

        Initiate_Contractor_Matric();
    }

    public void Initiate_Contractor_Matric()
    {
        //initiate and fill the edges list
        allEdges = new List<Edges>();
        Edges temp = new Edges();
        string[] tmpNodes = new string[3];
        GameObject[] theRedEdges = GameObject.FindGameObjectsWithTag("redLine");
        GameObject[] theBlueEdges = GameObject.FindGameObjectsWithTag("blueLine");
        GameObject[] theGreenEdges = GameObject.FindGameObjectsWithTag("greenLine");

        //print("rededges: " + theRedEdges.Length);
        //get all red edges
        for (int i = 0; i < theRedEdges.Length; i++)
        {
            tmpNodes = theRedEdges[i].name.Split('_');
            if (tmpNodes.Length > 2)
            {
                temp.contractor = 1;
                temp.id = i;
                temp.e1 = int.Parse(tmpNodes[2]);
                temp.e2 = int.Parse(tmpNodes[3]);

                allEdges.Add(temp);

            }
        }
        //get all blue edges
        for (int i = 0; i < theBlueEdges.Length; i++)
        {
            tmpNodes = theBlueEdges[i].name.Split('_');
            if (tmpNodes.Length > 2)
            {
                temp.contractor = 3;
                temp.e1 = int.Parse(tmpNodes[2]);
                temp.e2 = int.Parse(tmpNodes[3]);

                allEdges.Add(temp);

            }
        }
        //get all green edges
        for (int i = 0; i < theGreenEdges.Length; i++)
        {
            tmpNodes = theGreenEdges[i].name.Split('_');
            if (tmpNodes.Length > 2)
            {
                temp.contractor = 2;
                temp.e1 = int.Parse(tmpNodes[2]);
                temp.e2 = int.Parse(tmpNodes[3]);

                allEdges.Add(temp);

            }
        }

        //the contractors gets all noded
        AddNodesToContractor("redLine", ref Con_Red);
        AddNodesToContractor("blueLine", ref Con_Blue);
        AddNodesToContractor("greenLine", ref Con_Green);

        AddNodesToContractor("redLine", ref Con_All[0]);
        AddNodesToContractor("blueLine", ref Con_All[1]);
        AddNodesToContractor("greenLine", ref Con_All[2]);

        Contractor_Clusters_new(1);
        Contractor_Clusters_new(2);
        Contractor_Clusters_new(3);

        print("counting: " + allEdges.Count);
       // print(allEdges[allEdges.Count-1]);
        print("OK");
    }

    private int Get_Node_number_from_string(string value, int i)
    {
        char delimiter = '_';
        string[] substrings = value.Split(delimiter);
        if (i >= substrings.Length)
        {
            return -1;
        }
        else
        {
            return System.Int32.Parse(substrings[i]);
        }
    }

    private void AddNodesToContractor(string contractorTag, ref Contractor Con)
    {
        int tempVar = 0;
        int iLine = GameObject.FindGameObjectsWithTag(contractorTag).Length;
        Con.Init();
        for (int i = 0; i < iLine; i++)
        {
            ////////////////////////// add Edges
            tempVar = Get_Node_number_from_string(GameObject.FindGameObjectsWithTag(contractorTag)[i].name, 1); //Node 1 of the edge
            if (tempVar != -1)
            {
                if (!Con.Edges.Contains(tempVar))
                    Con.Edges.Add(tempVar);
            }
            //////////////////////// add Nodes
            tempVar = Get_Node_number_from_string(GameObject.FindGameObjectsWithTag(contractorTag)[i].name, 2); //Node 1 of the edge
            if (tempVar != -1)
            {
                if (!Con.Nodes.Contains(tempVar))
                    Con.Nodes.Add(tempVar);
            }
            tempVar = Get_Node_number_from_string(GameObject.FindGameObjectsWithTag(contractorTag)[i].name, 3); //Node 2 of the edge
            if (tempVar != -1)
            {
                if (!Con.Nodes.Contains(tempVar))
                    Con.Nodes.Add(tempVar);
            }

        }

    }


    private int Contractor_Clusters_new(int icontractor)
    {
        int preventInfLoop = 1;

        List<int> traversedEdges = new List<int>();
        List<int> temp;
        List<int> temp2;
        List<int> FinalClusters = new List<int>();
        List<int> traversHistory = new List<int>();
        List<int> TotalTraversHistory = new List<int>();
        List<Edges> ConEdges = new List<Edges>();

        string list_of_cluster_edges = "";


        for (int i = 0; i < allEdges.Count; i++)
        {
            if (allEdges[i].contractor == icontractor)
            {
                ConEdges.Add(allEdges[i]);
            }
        }


        for (int i = 0; i < ConEdges.Count; i++)
        {
            if (TotalTraversHistory.Contains(i)) { continue; }
            temp = GetConnections(i, ConEdges);
            while (temp.Count > 0)
            {

                for (int j = 0; j < temp.Count; j++)
                {
                    ////////////
                    preventInfLoop++;
                    if (preventInfLoop > 1000)
                    {
                        print(preventInfLoop);
                        return 0;
                    }
                    //////////////
                    if (!traversHistory.Contains(temp[j]))
                    {
                        temp2 = GetConnections(temp[j], ConEdges);
                        traversHistory.Add(temp[j]);
                        TotalTraversHistory.Add(temp[j]);
                        temp.RemoveAt(j);

                        for (int k = 0; k < temp2.Count; k++)
                        {
                            if (!traversHistory.Contains(temp2[k]))
                            {
                                temp.Add(temp2[k]);
                            }

                        }
                    }
                    else
                    {
                        temp.RemoveAt(j);
                    }


                }
            }
            //collectionOfTraversHistory.Add(traversHistory);
            if (traversHistory.Count > 0)
            {
                //print(traversHistory.Count); //count of clusters in each contractor
            }

            list_of_cluster_edges = "";
            for (int cls = 0; cls < traversHistory.Count; cls++)
            {
                FinalClusters.Add(traversHistory[cls]);


            }
            if (FinalClusters.Count > 0)
            {
                //.Clusters.Add(FinalClusters);

                for (int fnl = 0; fnl < FinalClusters.Count; fnl++)
                {

                    //    list_of_cluster_edges = list_of_cluster_edges+ "," + FinalClusters[fnl].ToString();

                    Con_All[icontractor - 1].Clusters.Add(FinalClusters[fnl]);
                }
            }
            print(Con_All[icontractor - 1].Clusters.Count);
            FinalClusters = new List<int>();

            traversHistory.Clear();
        }



        print("----");

        //for(int i=0;i< collectionOfTraversHistory.Count;i++)
        // {
        //     print(collectionOfTraversHistory[i].Count);
        //  }
        return 0;

    }

    private List<int> GetConnections(int ED1, List<Edges> ED2)
    {
        List<int> temp = new List<int>();
        for (int i = 0; i < ED2.Count; i++)
        {
            if (ED2[ED1].e1 == ED2[i].e1 ||
                ED2[ED1].e2 == ED2[i].e1 ||
                ED2[ED1].e1 == ED2[i].e2 ||
                ED2[ED1].e2 == ED2[i].e2)
            {
                temp.Add(i);
            }
        }
        return temp;

    }

    private void Contractor_Clusters(int icontractor)
    {

        //first filter allEdges to only red edges==>
        List<Edges> ConEdges = new List<Edges>();
        List<List<int>> Clusters_list = new List<List<int>>();
        List<int> tempList = new List<int>();

        for (int i = 0; i < allEdges.Count; i++)
        {
            if (allEdges[i].contractor == icontractor)
            {
                ConEdges.Add(allEdges[i]);
            }
        }
        print("over all there are " + ConEdges.Count + ", in contractor " + icontractor);
        /////////////////////////////////////////////////////////////////////
        // now from the red edges, create one cluster only
        List<List<Edges>> cluster = new List<List<Edges>>();
        // cluster.Add(redEdges[0]); //initiate cluster with one member only.
        //now find the connected elements to this single edge in this cluster
        int initialNumberOfEdges = ConEdges.Count;
        // add the first edge to a cluster 
        List<Edges> singleCluster = new List<Edges>();
        List<int> emptyObject = new List<int>();
        emptyObject.Add(0);
        singleCluster.Add(ConEdges[0]);
        cluster.Add(singleCluster);

        //   print("now we have "+cluster.Count + " clusters");

        List<List<int>> EDG_Connected_IDs = new List<List<int>>();

        int tempId = 0;
        int flag = 0;

        for (int i = 0; i < initialNumberOfEdges; i++)
        {
            flag = 0;
            emptyObject.Clear();
            //check all the links to an edge and add the edge id s to the list 
            for (int j = 0; j < initialNumberOfEdges; j++)
            {
                if (ConEdges[i].e1 == ConEdges[j].e1 ||
                    ConEdges[i].e2 == ConEdges[j].e1 ||
                    ConEdges[i].e1 == ConEdges[j].e2 ||
                    ConEdges[i].e2 == ConEdges[j].e2)
                {
                    tempId = ConEdges[j].id;
                    for (int o = 0; o < i; o++)
                    {
                        if (EDG_Connected_IDs[o].Contains(tempId))
                        {
                            flag = o;
                            break;
                        }
                    }
                    if (flag > 0)
                    {
                    }
                    else
                    {
                        emptyObject.Add(tempId);
                        EDG_Connected_IDs.Add(emptyObject);
                    }
                }
            }
        }
        for (int i = 0; i < EDG_Connected_IDs.Count; i++)
        {
            //       print("cluster " + i + " has " + EDG_Connected_IDs[i].Count);

        }
    }



    public void GetNumberofInterSections()
    {

        //find number of all edges.
        int NumberofEdges = allEdges.Count;
        int Nodes = allEdges[0].e1;
        for (int i = 0; i < allEdges.Count; i++)
        {
            if (Nodes < allEdges[i].e1) { Nodes = allEdges[i].e1; }
            if (Nodes < allEdges[i].e2) { Nodes = allEdges[i].e2; }
        }


        //initiate the table
        int[,] table;
        table = new int[Nodes + 1, 4];
        int[] tableSum = new int[Nodes + 1];
        string strT = "";
        string strTSum = "";



        //set the contractor Node Matrix
        for (int icontractor = 1; icontractor < 4; icontractor++)
        {
            for (int i = 0; i < NumberofEdges; i++)
            {
                if (allEdges[i].contractor == icontractor)
                {
                    table[allEdges[i].e1, icontractor] = 1;
                    table[allEdges[i].e2, icontractor] = 1;
                }
            }
        }

        //set the contractor Node Matrix
        for (int icontractor = 1; icontractor < 4; icontractor++)
        {
            for (int i = 1; i <= Nodes; i++)
            {

                strT = strT + table[i, icontractor];

                tableSum[i] = tableSum[i] + table[i, icontractor];
            }
            print("Contracotr " + icontractor + ":" + strT + "\n");
            strT = "";
        }

        for (int i = 1; i <= Nodes; i++)
        {
            strTSum = strTSum + tableSum[i];
        }
        print("Number of intersections per Node:" + strTSum);
    }



    // this function requires populating of the Con_red & Con_Blue na Con_green lists (Initiate_Contractor_Matric)
    public void ClusterDjikestra() 
    {
        string strTemp = "";
        float Times_for_each_contracor = 0.0f;
        float Debris_for_each_contracor = 0.0f;
        string Nclusters = "";

        for (int nc = 0; nc < 3; nc++)
        {
            for (int Assignment = 0; Assignment < Con_All[nc].Edges.Count; Assignment++)
            {
                strTemp = strTemp + " " + Con_All[nc].Edges[Assignment];
              //  Times_for_each_contracor += Manager.Instance.TimesList[Con_All[nc].Edges[Assignment]];
              //  Debris_for_each_contracor += Manager.Instance.debrisList[Con_All[nc].Edges[Assignment]];
            }

            /*
            for(int cluster=0; cluster<Con_All[nc].Clusters.Count; cluster++)
            {

               // Nclusters = Nclusters + Con_All[nc].Clusters[cluster];

            }

            print("The clusters " + nc + "=" + Nclusters);
            */

            print("The clusters of Contractor:" + Con_All[nc].Clusters.Count);
            print("The edges of Contractor " + nc + "=" + strTemp);
            print("The Time for contracotr " + nc + "=" + Times_for_each_contracor);
            print("The Debris for contracotr " + nc + "=" + Debris_for_each_contracor);
            strTemp = "";
            Nclusters = "";
            Times_for_each_contracor = 0.0f;
            Debris_for_each_contracor = 0.0f;
        }
    }

    public void TimeCalculation()
    {
        print("dijkstra calculation");
    }
    public void CostCalculation()
    {
        print("Cost calculation");
    }

}