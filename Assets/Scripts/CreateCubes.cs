using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCubes : MonoBehaviour {

    public GameObject cube;

    public float pos = 0;

    public float k = 10;

    int cubeCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
        {
            for (int i = 0; i < k; ++i)
            {
                GameObject inst = Instantiate(cube);
                ++pos; ++cubeCount;
            }
            Debug.Log("Cubes: " + cubeCount);
        }
	}
}
