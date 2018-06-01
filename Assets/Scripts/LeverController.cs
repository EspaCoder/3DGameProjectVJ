using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour {

    public List<GameObject> redstone;
    private bool on;

	// Use this for initialization
	void Start () {
        on = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void switchLever()
    {
        on = !on;
        if (on)
        {
            foreach (GameObject rs in redstone)
            {
                if (rs != null)
                {
                    rs.GetComponent<ParticleSystem>().Play();
                    gameObject.GetComponent<LeverAction1>().doAction();
                }
            }
        }
        else
        {
            foreach (GameObject rs in redstone)
            {
                if (rs != null)
                {
                    rs.GetComponent<ParticleSystem>().Clear();
                    rs.GetComponent<ParticleSystem>().Pause();
                }
            }
        }
    }
}
