using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

    public float speed = 0.00003f;
    public Vector3 direction = new Vector3(1, 0, 0);
    private Vector3 target;
    private string state;
    private float timeStabbed;
    public float lifeTimeInSeconds;
    public float lifeTimeInSteps;
    private float steps;

	// Use this for initialization
	void Start () {
        target = transform.position/* + direction * 3*/;
        state = "idle";
        direction = Vector3.forward;
        steps = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == "moving")
        {
            transform.Translate(speed * (target - transform.position), Space.World);
            //Debug.Log(target);
            //Debug.Log("Transform :" + transform);
            //Vector3 lookPosition = new Vector3(target.x - transform.position.x, this.transform.position.y, target.z - transform.position.z);
            //transform.LookAt(lookPosition);
            if (Mathf.Abs(target.x - transform.position.x) < 0.01 && Mathf.Abs(target.z - transform.position.z) < 0.01)
            {
                state = "idle";
            }
        }
        if ((state == "stabbed" && (Time.time - timeStabbed) >= lifeTimeInSeconds) || (state != "stabbed" && steps > lifeTimeInSteps))
        {
            Destroy(gameObject);
            //GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void move()
    {
        if (steps == 1)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        if (state == "idle" || state == "moving")
        {
            target = transform.position + transform.TransformDirection(direction) * 3;
            state = "moving";
            ++steps;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        state = "stabbed";
        timeStabbed = Time.time;
        if (collision.transform.tag == "Button")
        {
            transform.parent = collision.transform;
            collision.transform.gameObject.GetComponent<ButtonExample>().doAction();
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerController>().kill();
        }
    }


}
