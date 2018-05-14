using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float xHit, yHit, zHit;
    public float speed = 0.1f;
    string state = "idle";
    public Rigidbody rb;
    public float jumpForce = 1f;
    public float jumpY = 0.5f;

	// Use this for initialization
	void Start () {
        xHit = transform.position.x;
        yHit = transform.position.y;
        zHit = transform.position.z;
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(state);
        if (Mathf.Abs(xHit - transform.position.x) < 0.01 && Mathf.Abs(zHit - transform.position.z) < 0.01)
        {
            state = "idle";
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
            }
        }
        else
        {
            state = "walking";
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
                        (hit.point.y - (transform.position.y - 1f)) < 2)
                    {
                        xHit = hit.point.x;
                        yHit = hit.point.y;
                        zHit = hit.point.z;
                        int x = (int)xHit;
                        int y = (int)yHit;
                        int z = (int)zHit;
                        xHit = x + 0.5f;
                        zHit = z + 0.5f;
                    }
                }
            }
        }
        if (state == "walking")
        {
            transform.Translate((xHit - transform.position.x) * speed, 0, (zHit - transform.position.z) * speed);
            Vector3 origin = transform.position;
            origin.y -= 0.8f;
            Vector3 direction = new Vector3(xHit - transform.position.x, 0, zHit - transform.position.z);
            RaycastHit hitInfo;
            bool impact = Physics.Raycast(origin, direction, out hitInfo, 1f);
            if (impact && hitInfo.transform.tag == "Jumpable")
            {
                state = "jumping";
                Vector3 jumpingDir = direction;
                direction = direction / direction.sqrMagnitude;
                direction.y = jumpY;
                rb.AddForce(direction * jumpForce);
            }
        }
    }
}
