using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;
    public int healMin = 5;
    public int healMax = 15;
    public PlayerController playerSc;

    private void OnTriggerEnter(Collider other)
    {
        playerSc = FindObjectOfType<PlayerController>();
        if (other.tag == "Player")
        {
            healAmount = Random.Range(healMin, healMax);
            playerSc.currentHealth += healAmount;
            Destroy(gameObject);
        }
    }
}
