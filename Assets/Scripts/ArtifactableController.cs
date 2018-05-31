using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactableController : MonoBehaviour
{

    private GameObject player;
    private float speed;
    public float acceleration;
    private bool collected;
    private Inventory inventory;
    private AudioSource sound;
    public float rotationSpeed;
    private Vector3 initialPos;
    public float upDownSpeed;

    // Use this for initialization
    void Start()
    {
        speed = 0;
        collected = false;
        sound = GetComponent<AudioSource>();
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
        {
            Vector3 distance = player.transform.position - transform.position;
            Vector3 direction = Vector3.Normalize(distance);
            transform.Translate(direction * speed, Space.World);
            speed += acceleration;
            if ((Mathf.Abs(distance.x) < 1) && (Mathf.Abs(distance.y) < 1) && (Mathf.Abs(distance.z) < 1))
            {
                int nA = inventory.getnArtifactables();
                inventory.setnArtifactables(nA + 1);
                Destroy(gameObject);
            }
            if ((Mathf.Abs(distance.x) < 2) && (Mathf.Abs(distance.y) < 2) && (Mathf.Abs(distance.z) < 2))
            {
                if (!sound.isPlaying)
                    sound.Play();
            }
        }
        else
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            transform.position = initialPos + new Vector3(0, Mathf.Sin(upDownSpeed * Time.time) / 8, 0);
        }
    }

    public void collect(GameObject player)
    {
        this.player = player;
        inventory = player.GetComponent<Inventory>();
        collected = true;
    }
}
