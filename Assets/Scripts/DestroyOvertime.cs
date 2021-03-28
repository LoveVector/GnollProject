using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOvertime : MonoBehaviour
{
    public float lifetime;

    void Update() 
    {
        Destroy(gameObject, lifetime * Time.deltaTime);
    }
}
