using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour {

    private GameObject player;
    private float speed;
    public float acceleration;
    private bool collected;
    private Inventory inventory;
    private AudioSource sound;

	// Use this for initialization
	void Start () {
        speed = 0;
        collected = false;
        sound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (collected)
        {
            Vector3 distance = player.transform.position - transform.position;
            Vector3 direction = Vector3.Normalize(distance);
            transform.Translate(direction * speed);
            speed += acceleration;
            if ((Mathf.Abs(distance.x) < 1) && (Mathf.Abs(distance.y) < 1) && (Mathf.Abs(distance.z) < 1))
            {
                int nC = inventory.getnCollectibles();
                inventory.setnCollectibles(nC + 1);
                Destroy(gameObject);
            }
            if ((Mathf.Abs(distance.x) < 2) && (Mathf.Abs(distance.y) < 2) && (Mathf.Abs(distance.z) < 2))
            {
                if (!sound.isPlaying)
                    sound.Play();
            }
        }
	}

    public void collect (GameObject player)
    {
        this.player = player;
        inventory = player.GetComponent<Inventory>();
        collected = true;
    }
}
