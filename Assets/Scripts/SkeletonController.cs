using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

    public GameObject arrowObject;
    private List<GameObject> arrows;
    public int cadence;
    private int turnsSinceShot;

	// Use this for initialization
	void Start () {
        turnsSinceShot = 0;
        arrows = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void move ()
    {
        Debug.Log("Turns since shot: " + turnsSinceShot);
        foreach (GameObject arrow in arrows)
        {
            if (arrow != null)
            {
                arrow.GetComponent<ArrowController>().move();
            }
        }

        if (turnsSinceShot > cadence)
        {
            RaycastHit hitInfo;
            bool impact = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, 15f);
            if (impact && hitInfo.transform.tag == "Player")
            {
                GameObject arrow = Instantiate(arrowObject);
                arrow.transform.position = transform.position + transform.TransformDirection(Vector3.forward) * 1;
                arrow.transform.position = new Vector3(arrow.transform.position.x, 2, arrow.transform.position.z);
                arrow.transform.rotation = transform.rotation;
                arrows.Add(arrow);
                turnsSinceShot = 0;
            }
        }
        else
        {
            ++turnsSinceShot;
        }
    }
}
