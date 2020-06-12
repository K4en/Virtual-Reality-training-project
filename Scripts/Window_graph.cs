using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

public class Window_graph : MonoBehaviour
{
    private static Window_graph instance;

    [SerializeField] private Sprite dotSprite;
    private RectTransform graphContainer;
    private RectTransform labelText_x;
    private RectTransform labelText_y;
    private RectTransform dashLineX;
    private RectTransform dashLineY;
    private List<GameObject> gameObjectList;
    private List<IGraphVisualObject> graphVisualObjectList;
    private List<RectTransform> yLabelList;


    public IGraphVisual lineGraphVisual;
    public IGraphVisual barChartVisual;
    //Cached values

    private List<int> valueList;
    private float xSize;
    private bool startYScaleAtZero;
    private int maxVisibleValue;
    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;
    private IGraphVisual graphVisual;

    private void Awake()
    {
        instance = this;

        graphContainer = transform.Find("Container").GetComponent<RectTransform>();
        labelText_x = graphContainer.Find("LabelText_x").GetComponent<RectTransform>();
        labelText_y = graphContainer.Find("LabelText_y").GetComponent<RectTransform>();
        dashLineX = graphContainer.Find("dashLineX").GetComponent<RectTransform>();
        dashLineY = graphContainer.Find("dashLineY").GetComponent<RectTransform>();

        labelText_x.gameObject.SetActive(false);
        labelText_y.gameObject.SetActive(false);
        dashLineX.gameObject.SetActive(false);
        dashLineY.gameObject.SetActive(false);

        startYScaleAtZero = true;
        gameObjectList = new List<GameObject>();
        yLabelList = new List<RectTransform>();
        graphVisualObjectList = new List<IGraphVisualObject>();

        lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.blue, Color.white);
        barChartVisual = new BarChartVisual(graphContainer, Color.red, .8f);
        
        //IGraphVisual graphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.blue, Color.white);


        valueList = new List<int> { 0 };
    }

    public void SetGetAxisLabelX(Func<int, string> getAxisLabelX)
    {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValue, getAxisLabelX, this.getAxisLabelY);
    }

    public void SetGetAxisLabelY(Func<float, string> getAxisLabelY)
    {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValue, this.getAxisLabelX, getAxisLabelY);
    }

    public void IncreaseVisibleAmount()
    {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValue + 1, this.getAxisLabelX, this.getAxisLabelY);
    }

    public void DecreaseVisibleAmount()
    {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValue - 1, this.getAxisLabelX, this.getAxisLabelY);
    }


    public void ShowGraph(List<int> valueList, IGraphVisual graphVisual = null, int maxVisibleValue = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (valueList == null) return;
        this.valueList = valueList;

        if (graphVisual == null)
        {
            graphVisual = lineGraphVisual;
        }
        this.graphVisual = graphVisual;


        if (getAxisLabelX == null)
        {
            if (this.getAxisLabelX != null)
            {
                getAxisLabelX = this.getAxisLabelX;
            }
            else
            {
                getAxisLabelX = delegate (int _i) { return _i.ToString(); };
            }
        }
        if (getAxisLabelY == null)
        {
            if (this.getAxisLabelY != null)
            {
                getAxisLabelY = this.getAxisLabelY;
            }
            else
            {
                getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
            }
        }

        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;

        

        if (maxVisibleValue <= 0)
        {
            maxVisibleValue = valueList.Count;
        }
        if(maxVisibleValue > valueList.Count)
        {
            maxVisibleValue = valueList.Count;
        }

        this.maxVisibleValue = maxVisibleValue;

        //Destroying old/previous gameObjectList to create new graph based on new data
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        yLabelList.Clear();

        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectList)
        {
            graphVisualObject.CleanUp();
        }
        graphVisualObjectList.Clear();

        graphVisual.CleanUp();

        //Getting size parameters of the container object
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        //Size of X axis
        xSize = graphWidth / maxVisibleValue + 1;

        int xIndex = 0;

       
        GameObject lastDotGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValue, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            graphVisualObjectList.Add(graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize));
            //gameObjectList.AddRange(barChartVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize));
            //gameObjectList.Add(barGameObject);


            RectTransform labelX = Instantiate(labelText_x);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            RectTransform dashY = Instantiate(dashLineY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(xPosition, 5f);
            gameObjectList.Add(dashY.gameObject);
            xIndex++;
        }

        //Creating Y axis labels and dashes
        int seperatorCount = 10;
        for (int i = 0; i < seperatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelText_y);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedY = i * 1f / seperatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedY * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedY * (yMaximum - yMinimum)));
            yLabelList.Add(labelY);
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashX = Instantiate(dashLineX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(3, normalizedY * graphHeight);
            gameObjectList.Add(dashX.gameObject);
        }
    }

    public void UpdateLastIndexValue(int value)
    {
        UpdateValues(valueList.Count - 1, value);
    }

    private void UpdateValues(int index, int value)
    {
        float yMinimumBefore, yMaximumBefore;
        CalculateYScale(out yMinimumBefore, out yMaximumBefore);

        valueList[index] = value;

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        bool yScaleChanged = yMaximumBefore != yMinimum || yMaximumBefore != yMaximum;

        if (!yScaleChanged)
        {
            //Y scale did not change only update this value
            float xPosition = xSize + index * xSize;
            float yPosition = ((value - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            graphVisualObjectList[index].SetGraphVisualObjectinfo(new Vector2(xPosition, yPosition), xSize);
        }
        else
        {
            //Y scale changed update whole graph
            //Cycle through visible data points
            int xIndex = 0;
            for (int i = Mathf.Max(valueList.Count - maxVisibleValue, 0); i < valueList.Count; i++)
            {
                float xPosition = xSize + xIndex * xSize;
                float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

                graphVisualObjectList[xIndex].SetGraphVisualObjectinfo(new Vector2(xPosition, yPosition), xSize);


                xIndex++;
            }

            for (int i = 0; i < yLabelList.Count; i++)
            {
                float normalizedY = i * 1f / yLabelList.Count;
                yLabelList[i].GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedY * (yMaximum - yMinimum)));
            }
        }
        
    }

    private void CalculateYScale(out float yMinimum, out float yMaximum)
    {
        yMaximum = valueList[0];
        yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValue, 0); i < valueList.Count; i++)
        {
            int value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f); // increase maxY value so it doesnt touch top of graph
        yMinimum = yMinimum - (yDifference * 0.2f); // increase minY value so it doesnt touch bottom of graph

        if (startYScaleAtZero)
        {
            yMinimum = 0f;

        }
    }

    //create Interface

    public interface IGraphVisual
    {
        IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth);

        void CleanUp();
    }

    public interface IGraphVisualObject
    {
        void SetGraphVisualObjectinfo(Vector2 graphPosition, float graphPositionWidth);
        void CleanUp();
    }
    
    //Bar chart Visual class!!!

    private class BarChartVisual : IGraphVisual
    {
        private RectTransform graphContainer;
        private Color barColor;
        private float barWidthMultiplier;

        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier)
        {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
        }

        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth)
        {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);
            BarChartVisualObject barChartVisualObject = new BarChartVisualObject(barGameObject, barWidthMultiplier);
            barChartVisualObject.SetGraphVisualObjectinfo(graphPosition, graphPositionWidth);
            return barChartVisualObject;
        }
        private GameObject CreateBar(Vector2 graphPosition, float barWidth)
        {
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = barColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            return gameObject;
        }

        public void CleanUp()
        {
        }
        public class BarChartVisualObject : IGraphVisualObject
        {
            private GameObject barGameObject;
            private float barWidthMultiplier;

            public BarChartVisualObject(GameObject barGameObject, float barWidthMultiplier)
            {
                this.barGameObject = barGameObject;
                this.barWidthMultiplier = barWidthMultiplier;
            }


            public void SetGraphVisualObjectinfo(Vector2 graphPosition, float graphPositionWidth)
            {
                RectTransform rectTransform = barGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
                rectTransform.sizeDelta = new Vector2(graphPositionWidth * barWidthMultiplier, graphPosition.y);
            }

            public void CleanUp()
            {
                Destroy(barGameObject);
            }
        }
    }

    private class LineGraphVisual : IGraphVisual
    {
        private RectTransform graphContainer;
        private Sprite dotSprite;
        private LineGraphVisualObject lastLineGraphVisualObject;
        private Color dotColor;
        private Color lineColor;
        
        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color lineColor)
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            lastLineGraphVisualObject = null;
            this.lineColor = lineColor;
            this.dotColor = dotColor;
        }

        public void CleanUp()
        {
            lastLineGraphVisualObject = null;
        }
        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth)
        {
            List<GameObject> gameObjectList = new List<GameObject>();
            GameObject dotGameObject = CreateDot(graphPosition);

            gameObjectList.Add(dotGameObject);
            GameObject graphLineGameObject = null;
            if (lastLineGraphVisualObject != null)
            {
                graphLineGameObject = GraphLine(lastLineGraphVisualObject.GetGraphPosition(), dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(graphLineGameObject);
            }
           

            LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(dotGameObject, graphLineGameObject, lastLineGraphVisualObject);
            lineGraphVisualObject.SetGraphVisualObjectinfo(graphPosition, graphPositionWidth);

            lastLineGraphVisualObject = lineGraphVisualObject;
            return lineGraphVisualObject;
        }

        private GameObject GraphLine(Vector2 dotPositionA, Vector2 dotPositionB)
        {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = lineColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 direction = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + direction * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            return gameObject;
        }

        private GameObject CreateDot(Vector2 anchoredPosition)
        {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            return gameObject;

        }

        public class LineGraphVisualObject : IGraphVisualObject
        {
            public event EventHandler OnChangedGraphVisualObjectInfo;

            private GameObject dotGameObject;
            private GameObject dotLineGameObject;
            private LineGraphVisualObject lastVisualObject;

            public LineGraphVisualObject(GameObject dotGameObject, GameObject dotLineGameObject, LineGraphVisualObject lastVisualObject)
            {
                this.dotGameObject = dotGameObject;
                this.dotLineGameObject = dotLineGameObject;
                this.lastVisualObject = lastVisualObject;

                if(lastVisualObject != null)
                {
                    lastVisualObject.OnChangedGraphVisualObjectInfo += LastVisualObject_OnChangedGraphVisualObjectInfo;
                }
            }

            private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e)
            {
                UpDateLines();
            }

            public void SetGraphVisualObjectinfo(Vector2 graphPosition, float graphPositionWidth)
            {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = graphPosition;

                UpDateLines();

                if (OnChangedGraphVisualObjectInfo != null) OnChangedGraphVisualObjectInfo(this, EventArgs.Empty);
            }

            public void CleanUp()
            {
                Destroy(dotGameObject);
                Destroy(dotLineGameObject);
            }

            public Vector2 GetGraphPosition()
            {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                return rectTransform.anchoredPosition;
            }

            private void UpDateLines()
            {
                if (dotLineGameObject != null)
                {
                    RectTransform dotConnectionRectTransform = dotLineGameObject.GetComponent<RectTransform>();
                    Vector2 direction = (lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
                    float distance = Vector2.Distance(GetGraphPosition(), lastVisualObject.GetGraphPosition());
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
                    dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + direction * distance * .5f;
                    dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, angle);
                }
            }


        }
    }
}
