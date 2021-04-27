using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    Animator heartAnim;
    public PlayerController playerVal;

    // Start is called before the first frame update
    void Start()
    {
        heartAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        heartAnim.SetFloat("Health", playerVal.currentHealth);
    }
}
