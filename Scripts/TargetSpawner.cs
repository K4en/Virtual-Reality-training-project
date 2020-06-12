using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{

    public GameObject gameObject;

    //int enemiesToSpawn;
    public int enemiesSpawned = 0;

    [SerializeField] float timer = .5f;
    int spawnAtTime = 2;
   
    // Start is called before the first frame update

    private void Awake()
    {
        //enemiesToSpawn = FindObjectOfType<GameState>().enemyNumber;
    }
    void Start()
    {

        //InvokeRepeating("SpawnTarget", 1f, 1f);
        
    }


    private void SpawnTarget()
    {
        if (FindObjectOfType<TargetCounter>().enemiesToSpawn >= 0)
        {
            Instantiate(gameObject, new Vector3(Random.Range(-10, 15), Random.Range(2, 4), 4), gameObject.transform.rotation);
        }
        FindObjectOfType<TargetCounter>().enemiesToSpawn -= 1;
        
    }


    // Update is called once per frame
    void Update()
    {
        //enemiesToSpawn = FindObjectOfType<GameState>().enemyNumber;
        timer += Time.deltaTime;

        if (timer > spawnAtTime)
        {

            if (FindObjectOfType<TargetCounter>().enemiesToSpawn > 0)
            {

                SpawnTarget();
            }
            timer = 0;
        }

        //    if (enemiesToSpawn == 0)
        //{
        //    CancelInvoke();
        //}

        //Debug.Log(enemiesToSpawn);
    }

    //private void StartNewSpawn()
    //{
    //    spawnCoroutine = TimedSpawn();
    //    StartCoroutine(spawnCoroutine);
    //}

    //private IEnumerator TimedSpawn()
    //{
    //    while (true)
    //    {
    //        WaitForSeconds wait = new WaitForSeconds(2f);
    //        yield return new WaitForSeconds(3f);

    //        for (int i = enemiesToSpawn; i > 0; i--)
    //        {
    //            Instantiate(gameObject, new Vector3(Random.Range(-2, 4), Random.Range(2, 4), 4), gameObject.transform.rotation);

    //            yield return wait;
    //        }
    //}


}
