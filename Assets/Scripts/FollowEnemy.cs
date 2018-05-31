using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour {

    public GameObject player;
    public List<GameObject> traversableObjects;
    Dictionary<GameObject, float> distances;
    Dictionary<GameObject, GameObject> predecessors;
    private float xTarget, zTarget;
    public float speed = 0.1f;
    string state = "idle";

    private Vector3 currentVelocity;

    static Animator anim; 

    // Use this for initialization
    void Start() {
        xTarget = transform.position.x;
        zTarget = transform.position.z;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(xTarget - transform.position.x) < 0.01 && Mathf.Abs(zTarget - transform.position.z) < 0.01)
        {
            state = "idle";
            anim.SetBool("isWalking", false);
        }
        else
        {
            state = "walking";
            anim.SetBool("isWalking", true); 
        }
        if (state == "walking")
        {
            //transform.Translate((xTarget - transform.position.x) * speed, 0, (zTarget - transform.position.z) * speed, Space.World);
            Vector3 lookPosition = new Vector3(xTarget, this.transform.position.y, zTarget);
            transform.LookAt(lookPosition);

            Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 goalPosition = new Vector3(xTarget, transform.position.y, zTarget);

            transform.position = Vector3.SmoothDamp(currentPosition, goalPosition, ref currentVelocity, 0.3f);

            if (Mathf.Abs(xTarget - transform.position.x) < 0.4f && Mathf.Abs(zTarget - transform.position.z) < 0.4f)
            { 
                anim.SetBool("isWalking", false);
            }
        }
    }

    public void move ()
    {
        Vector3 origin = transform.position;
        origin.y += 0.5f;
        Vector3 direction = new Vector3(0, -1, 0);
        RaycastHit hitInfo;
        bool impact = Physics.Raycast(origin, direction, out hitInfo, 3.0f);
        if (impact)
        {
            GameObject startingCube = hitInfo.transform.gameObject;
            GameObject destCube = player.GetComponent<PlayerController>().getPlayerNextPositionCube();
            List<GameObject> occupiedCubes = player.GetComponent<PlayerController>().getOccupiedCubes();
            //BFS
            initStructures();
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(startingCube);
            distances[startingCube] = 0;
            bool found = false;
            GameObject currentCube;
            while (queue.Count > 0 && !found)
            {
                currentCube = queue.Dequeue();
                if (currentCube == destCube)
                    found = true;
                List<GameObject> adjacents = currentCube.GetComponent<AdjacencyList>().getAdjacencyList();
                foreach (GameObject adj in adjacents)
                {
                    if (adj != null && !occupiedCubes.Contains(adj))
                    {
                        if (distances[currentCube] + 1 < distances[adj] || distances[adj] == -1)
                        {
                            distances[adj] = distances[currentCube] + 1;
                            predecessors[adj] = currentCube;
                            queue.Enqueue(adj);
                        }
                    }
                }
            }
            if (found)
            {
                //Do the inverse traversal
                GameObject currentObj = destCube;
                while (predecessors[currentObj] != startingCube)
                {
                    currentObj = predecessors[currentObj];
                }
                xTarget = currentObj.transform.position.x;
                zTarget = currentObj.transform.position.z;
                player.GetComponent<PlayerController>().addOccupiedCube(currentObj);
            }
            Debug.Log("Dest :" + xTarget + " : " + zTarget);
        }
    }

    private void initStructures ()
    {
        distances = new Dictionary<GameObject, float>();
        predecessors = new Dictionary<GameObject, GameObject>();
        foreach (GameObject to in traversableObjects)
        {
            distances[to] = -1;
            predecessors[to] = null;
        }
    }

}
