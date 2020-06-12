using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI scoreText;
    //[SerializeField] int score = 5;
    [SerializeField] int scoreToAdd = 1;
    //[SerializeField] int pointsPerHit = 15;

    // Start is called before the first frame update
    void Start()
    {
        //scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScore()
    {
        FindObjectOfType<GameState>().AddScore(scoreToAdd);
    }

    public void DestroyTarget()
    {
        Destroy(this.gameObject);
    }

    public void EnemyCounter()
    {
        FindObjectOfType<GameState>().AddEnemyKilled(1);
    }

    //private void PointToAdd(int scoreToAdd)
    //{
    //    score += scoreToAdd;

    //}
}
