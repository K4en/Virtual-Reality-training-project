using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpehereBehavior : MonoBehaviour
{

    private float timer;
    private int speed = 2;

    private Vector3 tempPosition;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = MoveRandom(-2, 2);
        //tempPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            rb.velocity = MoveRandom(0, 0);
            
            timer = 0;
        }
        if(timer == 0)
        {
            rb.velocity = MoveRandom(-1, 1);
        }
    }

    private Vector3 MoveRandom(float min, float max)
    {
       var x = Random.Range(min, max);
       var y = Random.Range(min, max);
       var z = Random.Range(min, max);

        return new Vector3(x, y, z);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    rb.
    //}
}
