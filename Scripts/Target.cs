using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Target : MonoBehaviour
{

    public int score = 0;
    
    
    private void PointToAdd(int scoreToAdd)
    {
        score += scoreToAdd;
        
    }
}
