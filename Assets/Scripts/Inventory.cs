using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private int nCollectibles;
    private int nArtifactables;
    private bool artifactBuilt;

    // Use this for initialization
    void Start() {
        nCollectibles = 0;
    }

    // Update is called once per frame
    void Update() {

    }

    public int getnCollectibles()
    {
        return nCollectibles;
    }

    public void setnCollectibles(int nC)
    {
        nCollectibles = nC;
    }

    public int getnArtifactables()
    {
        return nArtifactables;
    }

    public void setnArtifactables(int nA)
    {
        nArtifactables = nA;
    }

    public void collectArtifact()
    {
        artifactBuilt = true;
    }
}
