using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private SpriteRenderer theSprite;

    void Start()
    {
        theSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.LookAt(PlayerController.instance.transform.position, -Vector3.forward);
    }
}
