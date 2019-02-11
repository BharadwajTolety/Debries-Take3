using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class LineChart : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    public RectTransform graphContainer;
    public RectTransform labelTemplateX;
    public RectTransform labelTemplateY;
    public RectTransform dashTemplateX;
    public RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;
    private List<float> valueList, valueList2;
    private int maxVisibleValue, maxVisibleValue2;
    private float values;

    private void Awake()
    {
        gameObjectList = new List<GameObject>();

        maxVisibleValue = 0;
        maxVisibleValue2 = 0;

        //The value to be input into the chart
        valueList = new List<float>(); //profit
        valueList2 = new List<float>(); //time


        //Create the graph, labelTemplateX and labelTemplateY
        //ShowGraph(valueList, (int _i) => "Iter." + (_i + 1));
    }

    //profit and then time
    public void reUpdate(float[] valueUpdate, float[] valueUpdate2)
    {
        valueList.Clear();
        valueList2.Clear();

        foreach (float value in valueUpdate)
        {
            valueList.Add(value);
        }

        foreach(float value in valueUpdate2)
        {
            valueList2.Add(value);
        }

        maxVisibleValue = valueList.Count;
        ShowGraph(valueList, maxVisibleValue, (int _i) => "Iter." + (_i + 1));
        ShowGraph(valueList2, maxVisibleValue2, (int _i) => "Iter." + (_i + 1));
    }

    //fix this function for new names
    public void update_graph()
    {
        if (this.name.Contains("current_"))
        {
            if (this.name.EndsWith("time"))
            {
                valueList.Add(Manager.Instance.cncTime[0]);
                maxVisibleValue++;
                valueList.Add(Manager.Instance.cncTime[1]);
                maxVisibleValue++;
                valueList.Add(Manager.Instance.cncTime[2]);
                maxVisibleValue++;

                ShowGraph(valueList, maxVisibleValue, (int _i) => "Iter." + (_i + 1));
            }
            else if (this.name.EndsWith("profit"))
            {
                valueList.Add(Manager.Instance.cncProfit[0]);
                maxVisibleValue++;
                valueList.Add(Manager.Instance.cncProfit[1]);
                maxVisibleValue++;
                valueList.Add(Manager.Instance.cncProfit[2]);
                maxVisibleValue++;

                ShowGraph(valueList, maxVisibleValue, (int _i) => "Iter." + (_i + 1));
            }
            else if (this.name.EndsWith("total"))
            {
                valueList.Add(Manager.Instance.maxProfit);
                maxVisibleValue++;
                valueList2.Add(Manager.Instance.minTime);
                maxVisibleValue++;

                ShowGraph(valueList, maxVisibleValue, (int _i) => "Iter." + (_i + 1));
                ShowGraph(valueList2, maxVisibleValue2, (int _i) => "Iter." + (_i + 1));
            }
            else
            {
                values = 0;
                valueList.Add(Manager.Instance.maxProfit);
                maxVisibleValue++;
                ShowGraph(valueList, maxVisibleValue, (int _i) => "Iter." + (_i + 1));
            }
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("Knob", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<float> valueList, int maxVisibleValueAmount, Func<int, string> getAxisLableX = null, Func<float, string> getAxisLableY = null)
    {
        if (getAxisLableX == null) {
            getAxisLableX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLableY == null) {
            getAxisLableY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }

        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount,0); i < valueList.Count; i++) {
            float value = valueList[i];
            if (value > yMaximum) {
                yMaximum = value;
            }
            if (value < yMinimum) {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        float xSize = graphWidth / maxVisibleValueAmount;

        int xIndex = 0;

        GameObject lastCircleGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum)/ (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circleGameObject);
            if (lastCircleGameObject != null)
            {
                GameObject dotConnecionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnecionGameObject);
            }
            lastCircleGameObject = circleGameObject;

            //Create the label for x axis
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLableX(i);
            gameObjectList.Add(labelX.gameObject);

            //Create the vertical dash
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -7f);
            gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        int separatorCount = 15;
        for (int i = 0; i <= separatorCount; i++) 
        {
            float normalizeValue = i * 1f / separatorCount;

            //Create the label for y axis
            /*
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-7f, normalizeValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLableY(yMinimum + normalizeValue * (yMaximum - yMinimum));
            gameObjectList.Add(labelY.gameObject);
            */

            //Create the horizontal dash
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizeValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);

        }
    }

    //Create the connection between dots
    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        return gameObject;
    }
}
