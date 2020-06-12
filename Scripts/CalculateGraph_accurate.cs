using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateGraph_accurate : MonoBehaviour
{
    Window_graph windowGraph;
    // Start is called before the first frame update
    private void Awake()
    {
        //gameState = GetComponent<GameState>();
        windowGraph = GameObject.Find("Graph object/Graph canvas/Graph_accurate").GetComponent<Window_graph>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        windowGraph.SetGetAxisLabelX((int _i) =>
        {
            string levelString = FindObjectOfType<GameState>().levelNumberList[_i].ToString();
            return "Level" + levelString;
        });

        windowGraph.SetGetAxisLabelY((float _i) =>
        {

            return "Acc " + "\n" + _i;
        });


        windowGraph.ShowGraph(FindObjectOfType<GameState>().accurateShotsList);
    }
}
