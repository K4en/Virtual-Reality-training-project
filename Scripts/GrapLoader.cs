using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapLoader : MonoBehaviour
{
    [SerializeField] GameObject graphScreen;

    // Start is called before the first frame update
    void Start()
    {
        graphScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            graphScreen.SetActive(true);
        }
        else
        {
            graphScreen.SetActive(false);
        }
    }
}
