using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistScript : MonoBehaviour
{
    int startIndex;
    private void Awake()
    {
        int sceneNumber = FindObjectsOfType<PersistScript>().Length;


        if (sceneNumber > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        startIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex != startIndex)
        {
            Destroy(gameObject);
        }
    }
}
   