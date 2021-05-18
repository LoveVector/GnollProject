using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTeleporter : MonoBehaviour
{
    public GameObject teleportSpot;

    private void Start()
    {
        teleportSpot = GameObject.Find("teleportSpot");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = teleportSpot.transform.position;
        }
    }
}
