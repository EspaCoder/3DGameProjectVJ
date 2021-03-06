﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    float xHit, zHit;
    public float speed = 0.1f;
    string state = "idle";
    public Rigidbody rb;
    public float jumpForce = 1f;
    public float jumpY = 0.5f;

    public GameObject AIMaster;
    private GameObject destCube;
    private Inventory inventory;
    public GameObject artifact;
    public float dieSpeed;
    private Quaternion deadRotation;

    //private Vector3 walkingInitialPos;
    private Vector3 currentVelocity;

    static Animator anim;

    // Use this for initialization
    void Start()
    {
        xHit = transform.position.x;
        //yHit = transform.position.y;
        zHit = transform.position.z;
        //walkingInitialPos = new Vector3(xHit, 0, zHit);
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1f;
        currentVelocity = Vector3.zero;
        inventory = GetComponent<Inventory>();
        deadRotation = Quaternion.AngleAxis(90, transform.TransformDirection(transform.forward));
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != "killed")
        {
            //Debug.Log(state);
            if (Mathf.Abs(xHit - transform.position.x) < 0.01 && Mathf.Abs(zHit - transform.position.z) < 0.01)
            {
                state = "idle";
                //walkingInitialPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
                currentVelocity = Vector3.zero;
                anim.SetBool("isWalking", false);
            }
            else if (state == "jumping")
            {
                Vector3 origin = transform.position;
                origin.y -= 1f;
                Vector3 direction = new Vector3(0, -1, 0);
                RaycastHit hitInfo;
                bool impact = Physics.Raycast(origin, direction, out hitInfo, 0.2f);
                if (impact)
                {
                    state = "walking";
                    anim.SetBool("isJumping", false);
                }
            }
            else
            {
                state = "walking";
                if (Mathf.Abs(xHit - transform.position.x) < 0.6 && Mathf.Abs(zHit - transform.position.z) < 0.6)
                    anim.SetBool("isWalking", false);
                else
                    anim.SetBool("isWalking", true);

            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Walkable" && ((Mathf.Abs(xHit - transform.position.x)) < 0.3 && ((Mathf.Abs(zHit - transform.position.z)) < 0.3))) //Get if clickable position
                    {
                        if (Mathf.Abs(hit.point.x - transform.position.x) + Mathf.Abs(hit.point.z - transform.position.z) < 4 &&
                            (hit.point.y - (transform.position.y - 1f)) < 2.5f)
                        {
                            xHit = hit.point.x;
                            //yHit = hit.point.y;
                            zHit = hit.point.z;
                            int x = (int)xHit;
                            //int y = (int)yHit;
                            int z = (int)zHit;
                            xHit = x + 0.5f;
                            zHit = z + 0.5f;
                            destCube = hit.transform.gameObject;
                            AIMaster.GetComponent<AIMaster>().move();
                        }
                    }

                    else if (hit.transform.tag == "Collectible")
                    {
                        hit.transform.GetComponent<CollectibleController>().collect(gameObject);
                    }

                    else if (hit.transform.tag == "Artifactable")
                    {
                        hit.transform.GetComponent<ArtifactableController>().collect(gameObject);
                    }

                    else if (hit.transform.tag == "CraftingTable")
                    {
                        Debug.Log("Crafting!");
                        int nA = inventory.getnArtifactables();
                        if (nA == 3)
                        {
                            Debug.Log("Making the sword!");
                            GameObject obj = Instantiate(artifact);
                            obj.transform.position = new Vector3((int)hit.transform.position.x + 0.5f, hit.transform.position.y, (int)hit.transform.position.z + 0.5f) + (new Vector3(0, 1, 0));
                            inventory.setnArtifactables(0);
                        }
                    }
                    else if (hit.transform.tag == "Artifact")
                    {
                        hit.transform.GetComponent<ArtifactController>().collect(gameObject);
                        inventory.collectArtifact();
                        //Destroy(hit.transform.gameObject);
                    }
                    else if (hit.transform.tag == "Lever")
                    {
                        hit.transform.GetComponent<LeverController>().switchLever();
                    }
                }

            }
            Vector3 originKey = transform.position;
            originKey.y += 0.4f;
            Vector3 directionKey = Vector3.down;
            RaycastHit hitInfoKey;
            bool impactKey = Physics.Raycast(originKey, directionKey, out hitInfoKey, 0.8f);
            List<GameObject> adjList = null;
            if (impactKey)
            {
                GameObject currentCube = hitInfoKey.transform.gameObject;
                if (currentCube.tag == "Walkable")
                {
                    adjList = currentCube.GetComponent<AdjacencyList>().getPlayerAdjacencyList();
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (adjList != null && adjList[0] != null)
                {
                    xHit = adjList[0].transform.position.x;
                    zHit = adjList[0].transform.position.z;
                    AIMaster.GetComponent<AIMaster>().move();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (adjList != null && adjList[1] != null)
                {
                    xHit = adjList[1].transform.position.x;
                    zHit = adjList[1].transform.position.z;
                    AIMaster.GetComponent<AIMaster>().move();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (adjList != null && adjList[2] != null)
                {
                    xHit = adjList[2].transform.position.x;
                    zHit = adjList[2].transform.position.z;
                    AIMaster.GetComponent<AIMaster>().move();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (adjList != null && adjList[3] != null)
                {
                    xHit = adjList[3].transform.position.x;
                    zHit = adjList[3].transform.position.z;
                    AIMaster.GetComponent<AIMaster>().move();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {

            }
            if (state == "walking")
            {
                //Debug.Log("X: " + xHit + " Z:" + zHit);
                Vector3 lookPosition = new Vector3(xHit, this.transform.position.y, zHit);
                transform.LookAt(lookPosition);

                Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Vector3 goalPosition = new Vector3(xHit, transform.position.y, zHit);

                transform.position = Vector3.SmoothDamp(currentPosition, goalPosition, ref currentVelocity, 0.3f);

                //Alternative with starting speed close to c
                //transform.Translate((xHit - transform.position.x) * speed, 0, (zHit - transform.position.z) * speed, Space.World);

                Vector3 origin = transform.position;
                origin.y += 0.4f;
                Vector3 direction = new Vector3(xHit - transform.position.x, 0, zHit - transform.position.z);
                RaycastHit hitInfo;
                bool impact = Physics.Raycast(origin, direction, out hitInfo, 0.8f);
                if (impact && hitInfo.transform.tag == "Jumpable")
                {
                    state = "jumping";
                    anim.SetBool("isJumping", true);
                    //Vector3 jumpingDir = direction;
                    direction = direction / direction.sqrMagnitude;
                    //direction.y = jumpY;
                    //direction = direction / direction.sqrMagnitude;
                    Debug.Log("Direction x: " + direction.x + " Direction z: " + direction.z);
                    if (Mathf.Abs(direction.x) < 0.05)
                    {
                        direction = Quaternion.Euler(-jumpY, 0.0f, 0.0f) * direction;
                    }
                    else if (Mathf.Abs(direction.z) < 0.05)
                    {
                        direction = Quaternion.Euler(0.0f, 0.0f, -jumpY) * direction;
                    }
                    direction.y = Mathf.Abs(direction.y);
                    Debug.Log("Direction : " + direction);
                    rb.AddForce(direction * jumpForce);
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, deadRotation, Time.deltaTime * dieSpeed);
            Debug.Log("Rotating");
            if (Quaternion.Angle(transform.rotation, deadRotation) < 10f)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Patrol" || collision.transform.tag == "Follow")
        {
            Destroy(gameObject);
        }
    }

    public GameObject getPlayerNextPositionCube()
    {
        return destCube;
    }

    public List<GameObject> getOccupiedCubes()
    {
        return AIMaster.GetComponent<AIMaster>().getOccupiedCubes();
    }

    public void addOccupiedCube(GameObject cube)
    {
        AIMaster.GetComponent<AIMaster>().addOccupiedCube(cube);
    }

    public void kill()
    {
        state = "killed";
    }

}

