using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

    public GameObject arrowObject;
    private List<GameObject> arrows;
    public int cadence;
    private int turnsSinceShot;
    static Animator anim;
    private bool charged;
    public Animator bowAnim;
    public int facingDirection;

    private Vector3 arrowPoint; 

    // Use this for initialization
    void Start () {
        turnsSinceShot = 2;
        arrows = new List<GameObject>();
        anim = GetComponent<Animator>();
        charged = false; 
        if (facingDirection == 0)
        {
           arrowPoint = new Vector3(0.252f, 1.436159f, 0.484f);
        }
        else if (facingDirection == 90)
        {
            arrowPoint = new Vector3(0.5f, 1.439116f, -0.22f);
        }
        else if (facingDirection == 180)
        {
            arrowPoint = new Vector3(-0.232f, 1.4491159f ,-0.515f);
        }
        else
        {
            arrowPoint = new Vector3(-0.645f, 1.39416f, 0.303f); 
        }


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
                if (charged)
                {
                    charged = false;
                    anim.SetBool("isDrawing", false);
                    bowAnim.SetBool("isDrawing", false);
                }
                arrow.GetComponent<ArrowController>().move();
            }
        }

        if (turnsSinceShot > cadence)
        {
            RaycastHit hitInfo;
     
            Vector3 origin = transform.position;
            origin.y += 0.5f; 
            bool impact = Physics.Raycast(origin, transform.TransformDirection(Vector3.forward), out hitInfo, 15f);
            if (impact && hitInfo.transform.tag == "Player")
            {
                anim.SetBool("isDrawing", true);
                bowAnim.SetBool("isDrawing", true);

                GameObject arrow = Instantiate(arrowObject);

                arrow.transform.position = transform.position + arrowPoint; 
                arrow.transform.rotation = transform.rotation; 

                arrows.Add(arrow);
                turnsSinceShot = 0;
           
                charged = true; 
            }
        }
        else
        {
            ++turnsSinceShot;
        }
    }
}
