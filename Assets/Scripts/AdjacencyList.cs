﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacencyList : MonoBehaviour {

    public List<GameObject> adjacencyList;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<GameObject> getAdjacencyList ()
    {
        return adjacencyList;
    }
}
