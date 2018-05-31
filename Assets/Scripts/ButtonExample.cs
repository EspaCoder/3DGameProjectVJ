using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExample : MonoBehaviour {

    private string state = "idle";
    private Vector3 initialPos;
    public float speed = 0.01f;
    private float timeStop;
    public float timeStopped;

	// Use this for initialization
	void Start () {
        initialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == "moveBack")
        {
            Debug.Log("Move back");
            transform.Translate(new Vector3(0, 0, -1) * speed, Space.Self);
            if (Mathf.Abs(transform.position.x - initialPos.x) > 0.05f || Mathf.Abs(transform.position.z - initialPos.z) > 0.05f)
            {
                state = "stopped";
                timeStop = Time.time;
            }
        }
        else if (state == "stopped")
        {
            if ((Time.time - timeStop) >= timeStopped)
            {
                state = "moveForward";
            }
        }
        else if (state == "moveForward")
        {
            Debug.Log("Move forward");
            transform.Translate(new Vector3(0, 0, 1) * speed, Space.Self);
            if (Mathf.Abs(transform.position.x - initialPos.x) < 0.01f && Mathf.Abs(transform.position.z - initialPos.z) < 0.01f)
            {
                state = "idle";
            }
        }
	}

    public void doAction()
    {
        //Do whatever
        Debug.Log("Doing button stuff");
        if (state == "idle")
        {
            state = "moveBack";
        }
    }
}
