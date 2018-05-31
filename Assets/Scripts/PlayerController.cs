using System.Collections;
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
    public GameObject collectibleBuilt;

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

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetMouseButtonDown(0) && ((Mathf.Abs(xHit - transform.position.x)) < 0.3 && ((Mathf.Abs(zHit - transform.position.z)) < 0.3)))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Walkable") //Get if clickable position
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

                else if (hit.transform.tag == "CraftingTable")
                {
                    Debug.Log("Crafting!");
                    int nC = inventory.getnCollectibles();
                    if (nC == 3)
                    {
                        Debug.Log("Making the sword!");
                        GameObject obj = Instantiate(collectibleBuilt);
                        obj.transform.position = hit.transform.position + (new Vector3(0, 1, 0));
                        inventory.setnCollectibles(0);
                    }
                }
                else if (hit.transform.tag == "CollectibleBuilt")
                {
                    inventory.collectBuilt();
                    Destroy(hit.transform.gameObject);
                }
            }

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

}

