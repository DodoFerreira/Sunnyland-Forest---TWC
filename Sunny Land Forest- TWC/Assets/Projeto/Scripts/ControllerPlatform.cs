using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlatform : MonoBehaviour
{

public Transform platform, pointA, pointB;
public float speed;
public Vector3 destiny;

public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        platform.position = pointA.position;
        destiny = pointB.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (platform.position == pointA.position)
        {
            StartCoroutine ("DelayA");
            
            
        }

        if (platform.position == pointB.position)
        {
            StartCoroutine ("DelayB");
            
        }

        platform.position = Vector3.MoveTowards (platform.position, destiny, speed);
    }

    IEnumerator DelayA ()
    {
        yield return new WaitForSeconds (.2f);
        destiny = pointB.position;
    }

    IEnumerator DelayB ()
    {
        yield return new WaitForSeconds (.2f);
        destiny = pointA.position;
    }
}
