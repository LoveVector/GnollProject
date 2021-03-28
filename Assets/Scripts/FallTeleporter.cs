using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTeleporter : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = new Vector3(0, 3, 0);
        }
    }
}
