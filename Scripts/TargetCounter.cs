using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCounter : MonoBehaviour
{
    public int enemiesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        enemiesToSpawn = FindObjectOfType<GameState>().enemyNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
