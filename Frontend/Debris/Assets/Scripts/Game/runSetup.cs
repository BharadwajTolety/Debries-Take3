using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using System;

//setting up run system front and back both
public class runSetup : MonoBehaviour
{
    private bool takescreenshotonNextFrame = false;
    private Camera gameCamera;

    private string JSONstring;
    private static JsonData mapItem;

    private static runSetup instance;
    private static int x_, y_, w_, h_;

    private void Awake()
    {
        gameCamera = gameObject.GetComponent<Camera>();
        instance = this;
        string log_directory = Application.persistentDataPath + "/run_images";
        if (!Directory.Exists(log_directory))
        {
            Directory.CreateDirectory(log_directory);
        }
    }

    private void Start()
    {
        JSONstring = File.ReadAllText(Manager.Instance.map_json);

        mapItem = JsonMapper.ToObject(JSONstring);
    }

    private void OnPostRender()
    {
        string run_image = Application.persistentDataPath + "/run_images" + "/run_" + Manager.Instance.run + ".png";

        if(takescreenshotonNextFrame)
        {
            takescreenshotonNextFrame = false;
            RenderTexture renderTex = gameCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGBAFloat, false);
            Rect rect = new Rect(x_, y_, w_, h_);

            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArr = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(run_image,byteArr);

            Debug.Log("image saved for run:" + Manager.Instance.run);
            Debug.Log(run_image);

            RenderTexture.ReleaseTemporary(renderTex);
            gameCamera.targetTexture = null;
        }
    }

    private void takeScreenShot(int width, int height)
    {
        gameCamera.targetTexture = RenderTexture.GetTemporary(width,height,16);
        takescreenshotonNextFrame = true;
    }

    public static void log_data(string path, int prft_obj, int time_obj, int inter_obj)
    {
        instance.logging(path, prft_obj, time_obj, inter_obj);
    }

    private void logging(string path, int prft_obj, int time_obj, int inter_obj)
    {
        StringBuilder csv = new StringBuilder();

        string line, suggest, inputObj;
        int source, dest, number_of_edges = mapItem["EdgeData"].Count;

        //read the brushed_edges from matlab to log them here
        string scanFile = Application.streamingAssetsPath + "/Database/Input/brushedEdges_Matlab.csv";
        string[] readBrush = new string[number_of_edges + 1]; //+1 just to be extra safe but the brushfile cannot have more lines than number of edges
        bool brush_error = false;
        try
        {
            //read brushed file
            readBrush = File.ReadAllLines(scanFile);
            Debug.Log("read brushed file");
            brush_error = false;
        }
        catch (Exception e)
        {
            brush_error = true;
            Debug.Log("the file couldnt be read - " + e.Message);
        }

        //add the header
        if (Manager.Instance.scans == 1)
        {
            line = "-,time_played,MaxProfit,MinTime,red_profit,red_time,green_profit,green_time,blue_profit,blue_time,intersect,total_suggest,input_obj";

            for (int i = 0; i < number_of_edges; i++)
            {
                source = (int)mapItem["EdgeData"][i]["From"];
                dest = (int)mapItem["EdgeData"][i]["To"];
                line += ',' + source + '_' + dest;
            }
                csv.AppendLine(line);
        }
        
        //add all info in for this scan
        line = "scans_" + Manager.Instance.scans + ',' + Manager.Instance.time_played.ToString() + ',' + Manager.Instance.maxProfit + ',' + Manager.Instance.minTime;

        for (int i = 0; i < 3; i++)
        {
            line += ',' + Manager.Instance.cncProfit[i].ToString() + ',' + Manager.Instance.cncTime[i].ToString();
        }

        suggest = Manager.Instance.suggest[0].ToString() + Manager.Instance.suggest[1].ToString() + Manager.Instance.suggest[2].ToString();
        inputObj = prft_obj.ToString() + time_obj.ToString() + inter_obj.ToString();

        line += ',' + Manager.Instance.intersect + ',' + suggest + ',' + inputObj;

        for (int i = 0; i < number_of_edges; i++)
        {
            source = (int)mapItem["EdgeData"][i]["From"];
            dest = (int)mapItem["EdgeData"][i]["To"];
            int edgeno = i + 1;
            GameObject edge = GameObject.Find("E_" + edgeno + "_" + source + "_" + dest);
            string nc = "";

            //checking player inputs for colors
            if (edge.tag == "white")
                nc = "";
            else
            {
                if (edge.tag.Contains("red"))
                    nc += '1';
                if (edge.tag.Contains("green"))
                    nc += '2';
                if (edge.tag.Contains("blue"))
                    nc += '3';
            }

            //checking matlab brush file results for this scan
            bool found_brush = false;
            if(!brush_error)
            {
                string[] brush_info = new string[3];
                foreach(string brush_edge in readBrush)
                {
                    brush_info = brush_edge.Split(',');
                    if((source + "_" + dest) == (brush_info[0] + "_" + brush_info[1]))
                    {
                        line += ',' + nc + '-' + brush_info[2];
                        found_brush = true;
                        break;
                    }
                }
            }
            
            //if no matlab brush info found on this edge just add player input
            if(!found_brush)
                line += ',' + nc;
        }
        csv.AppendLine(line);

        File.AppendAllText(path, csv.ToString());
    }

    public static void takeScreenShot_static(int width, int height, int x, int y, int w, int h)
    {
        instance.takeScreenShot(width , height);

        w_ = w;
        h_ = h;
        x_ = x;
        y_ = y;
    }
}
