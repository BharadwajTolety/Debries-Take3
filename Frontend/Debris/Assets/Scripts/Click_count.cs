using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Click_count : MonoBehaviour
{
	//Highlight select
	private int[] count;
    /*private int count_reye = 0;//0
    private int count_beye = 0;//1
    private int count_geye = 0;//2
	//Brush select
	private int count_r = 0;//3
	private int count_b = 0;//4
	private int count_g = 0;//5
	private int count_unselect = 0;//6
    //Algo buttons
	private int count_scan = 0;//8
	private int count_advice = 0;//7
	private int count_clock = 0;//9
	private int count_money = 0;//10
	private int count_intersect = 0;//11
    //Misc
	private int count_undo = 0;//12
	private int count_redo = 0;//13*/
							   //File 
	string writepath;
	string[] click_cnt;

	public void init_array()
	{
		count = new int[15];
        for (int i = 0; i < 15; i++)
        {
            count[i] = 0;
        }

	}
	public void init_string()
	{
		click_cnt[0] = "Red eye: ";
		click_cnt[1] = "Blue eye: ";
		click_cnt[2] = "Green eye: ";
		click_cnt[3] = "Red button: ";
		click_cnt[4] = "Blue button: ";
		click_cnt[5] = "Green button: ";
		click_cnt[6] = "Unselect button: ";
		click_cnt[7] = "Scan button: ";
		click_cnt[8] = "Advice button: ";
		click_cnt[9] = "Time button: ";
		click_cnt[10] = "Money button: ";
		click_cnt[11] = "Intersect button: ";
		click_cnt[12] = "Undo button: ";
		click_cnt[13] = "Redo button: ";
	}
   

    public void press_count(int button_no)
	{
		switch(button_no)
		{
			case 0:
				count[0]++;break;
			case 1:
				count[1]++;break;
			case 2:
				count[2]++;break;
			case 3:
				count[3]++;break;
			case 4:
				count[4]++;break;
			case 5:
				count[5]++;break;
			case 6:
				count[6]++;break;
			case 7:
				count[7]++;break;
			case 8:
				count[8]++;break;
			case 9:
				count[9]++;break;
			case 10:
				count[10]++;break;
			case 11:
				count[11]++;break;
			case 12:
				count[12]++;break;
			case 13:
				count[13]++;break;
		}
	}
	void Writefile(string path)
    {
        StreamWriter swrite;
        if (!File.Exists(path))
        {
            swrite = File.CreateText(Application.dataPath + "/Write_Info.txt");
        }
        else
        {
			swrite = new StreamWriter(path,append:true);
        }
		//Combining string and counts
		for (int i = 0; i < 14;i++)
		{
			click_cnt[i] = click_cnt[i] + count[i].ToString();
		}
		swrite.WriteLine(count[3]);
		swrite.WriteLine(click_cnt[3]);
		/*for (int i = 0; i < 14;i++)
		{
			swrite.WriteLine(count[i]);
		}*/
        swrite.Close();
    }
    void Start()
    {
        writepath = Application.dataPath + "/Write_Info.txt";
		init_array();
		init_string();
        //Writefile(writepath);

    }
	private void OnApplicationQuit()
	{
		Writefile(writepath);
	}
	void Update () 
	{
		Debug.Log("Red button pressed : " + count[3]);
		//Debug.Log("Green button pressed : " + count_g);
		//Debug.Log("Blue button pressed : " + count_b);
	}
}
