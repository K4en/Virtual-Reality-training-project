using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class GameState : MonoBehaviour
{

    [SerializeField] int score = 0;

    [SerializeField] public int enemiesKilled = 0;
    [SerializeField] public int goal = 3;
    [SerializeField] public int enemyNumber = 3;

    [SerializeField] public int stageNumber = 0;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI goalText;
    [SerializeField] TextMeshProUGUI notificationText;
    [SerializeField] TextMeshProUGUI notificationNumber;

    [SerializeField] TextMeshProUGUI timeText;


    Window_graph windowGraph;

    public bool isTargetAchieved;
    // Start is called before the first frame update

    public float timer;
    public float waitingTimeForLoad;
    public int levelNumber = 0;

    public List<int> timeList;
    public List<int> levelNumberList;
    public List<int> accurateShotsList;
    public List<int> shotsFiredList;

    int sceneIndex;
    int prevScene;
    //public int numberOfGames;
    private void Awake()
    {
        //Start each list for the graph with 0
        timeList = new List<int> { 0 };
        levelNumberList = new List<int> { 0 };
        accurateShotsList = new List<int> { 0 };
        shotsFiredList = new List<int> { 0 };

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        prevScene = SceneManager.GetActiveScene().buildIndex - 1;

        //int numberOfGames = FindObjectsOfType<GameState>().Length;
        //if (numberOfGames > 1)
        //{
        //    Destroy(gameObject);
            
        //}
        //else
        //{
        //    DontDestroyOnLoad(gameObject);
        //}
    }

    void Start()
    {
        //notificationNumber.enabled = false;
        //notificationText.enabled = false;

        scoreText.text = enemiesKilled.ToString();
        goalText.text = goal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Counting timer.
        timer += Time.deltaTime;
        timeText.text = timer.ToString("#.##");
        //Debug.Log(isTargetAchieved);
        if (enemiesKilled >= goal)
        {
            

            

            waitingTimeForLoad += Time.deltaTime;
            if (waitingTimeForLoad > 2f )
            {
                //Adding data to the lists for the graph
                levelNumber += 1;
                timeList.Add(Mathf.RoundToInt(timer));
                levelNumberList.Add(levelNumber);
                accurateShotsList.Add(enemiesKilled);
                shotsFiredList.Add(FindObjectOfType<RayCastShoot>().numberOfShots);
                FindObjectOfType<RayCastShoot>().numberOfShots = 0;
                //If all current target killed reset the screen. Update or reset target values. Display current score on screen.
                var currentScene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentScene);
                enemyNumber += 2;
                goal += 2;
                enemiesKilled = 0;
                timer = 0;
                scoreText.text = enemiesKilled.ToString();
                goalText.text = goal.ToString();

                waitingTimeForLoad = 0;
                StartCoroutine(WaitNextLevel((stageNumber += 1).ToString(), 1.5f));

                
            }

            
            //notificationText.text = "Target achieved";
        }

        

        
    }

    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void AddEnemyKilled(int EnemyKilled)
    {
        enemiesKilled += EnemyKilled;
        scoreText.text = enemiesKilled.ToString();
    }

    
    IEnumerator WaitNextLevel(string levelNumber, float delay)
    {
        notificationNumber.text = levelNumber;
        notificationNumber.gameObject.SetActive(true);

        notificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        notificationNumber.gameObject.SetActive(false);
        notificationText.gameObject.SetActive(false);

        
    }
}
