using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMaster : MonoBehaviour {

    public List<GameObject> AIList;
    private List<GameObject> occupiedCubes;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void move ()
    {
        occupiedCubes = new List<GameObject>();
        foreach (GameObject AIagent in AIList)
        {
            if (AIagent != null)
            { //Would be better to unattach
                if (AIagent.tag == "Follow")
                {
                    AIagent.GetComponent<FollowEnemy>().move();
                }
                else if (AIagent.tag == "Patrol")
                {
                    AIagent.GetComponent<PatrolEnemy>().move();
                }
                else if (AIagent.tag == "Arrow")
                {
                    AIagent.GetComponent<ArrowController>().move();
                }
                else if (AIagent.tag == "Skeleton")
                {
                    AIagent.GetComponent<SkeletonController>().move();
                }
            }
        }
    }

    public List<GameObject> getOccupiedCubes()
    {
        return occupiedCubes;
    }

    public void addOccupiedCube (GameObject cube)
    {
        occupiedCubes.Add(cube);
    }
} 
