using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpCollider : MonoBehaviour
{
    private PlayerController parent;

    void Start()
    {
        parent = transform.parent.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider aCol)
    {
        //parent.(collider, aCol); // pass the own collider and the one we've hit
    }
}
