using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{

    public GameObject player;
    public List<GameObject> waypoints;
    private int index;
    private float xTarget, zTarget;
    public float speed = 0.1f;
    string state;
    int explodeState;
    bool warming;
    public float growSpeed;

    private Vector3 currentVelocity;

    // Use this for initialization
    void Start()
    {
        xTarget = transform.position.x;
        zTarget = transform.position.z;
        index = 0;
        state = "idle";
        explodeState = 0;
        warming = false;

        currentVelocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(xTarget - transform.position.x) < 0.01 && Mathf.Abs(zTarget - transform.position.z) < 0.01)
        {
            state = "idle";
        }
        else
        {
            state = "walking";
        }
        if (state == "walking")
        {
            //transform.Translate((xTarget - transform.position.x) * speed, 0, (zTarget - transform.position.z) * speed, Space.World);

            Vector3 lookPosition = new Vector3(xTarget, this.transform.position.y, zTarget);
            transform.LookAt(lookPosition); 

            Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 goalPosition = new Vector3(xTarget, transform.position.y, zTarget);

            transform.position = Vector3.SmoothDamp(currentPosition, goalPosition, ref currentVelocity, 0.3f);
        }
        if (warming && (transform.localScale.x < 1.2 /*HARDCODED*/))
        {
            //transform.localScale += new Vector3(growSpeed, growSpeed, growSpeed);
        }
        if (!warming && (transform.localScale.x > 0.8 /*HARDCODED*/))
        {
            //transform.localScale -= new Vector3(growSpeed, growSpeed, growSpeed);
        }
        if (explodeState >= 3)
        {
            Destroy(player.transform.gameObject);
            Destroy(gameObject);
        }
        Debug.Log(state + " " + explodeState);
    }

    public void move ()
    {
        List<GameObject> occupiedCubes = player.GetComponent<PlayerController>().getOccupiedCubes();
        if (!occupiedCubes.Contains(waypoints[(index + 1) % waypoints.Capacity]))
        {
            index = (index + 1) % waypoints.Capacity;
            GameObject nextPosCube = waypoints[index];
            xTarget = nextPosCube.transform.position.x;
            zTarget = nextPosCube.transform.position.z;
            player.GetComponent<PlayerController>().addOccupiedCube(nextPosCube);
        }
        if (Vector3.Distance(player.transform.position, transform.position) < 3.5)
        {
            ++explodeState;
            warming = true;
        }
        else
        {
            if (explodeState > 0)
            {
                --explodeState; //Or 0?
            }
            warming = false;
        }
        Debug.Log(explodeState);
    }
}

