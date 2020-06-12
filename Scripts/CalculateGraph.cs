using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateGraph : MonoBehaviour
{

    //GameState gameState;

    Window_graph windowGraph;
    // Start is called before the first frame update
    private void Awake()
    {
        //gameState = GetComponent<GameState>();
        windowGraph = GameObject.Find("Graph object/Graph canvas/Graph").GetComponent<Window_graph>();
    }

    void Start()
    {
        windowGraph.ShowGraph(FindObjectOfType<GameState>().shotsFiredList);
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

            return "Sec " + "\n" + _i;
        });


        windowGraph.ShowGraph(FindObjectOfType<GameState>().timeList);
    }
}
