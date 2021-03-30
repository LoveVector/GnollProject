using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 25;
    public Shooting shootingSc;

    private void OnTriggerEnter(Collider other)
    {
        shootingSc = other.GetComponentInChildren<Shooting>();
        if(other.tag == "Player")
        {
            shootingSc.currentAmmo += ammoAmount;
            Destroy(gameObject);
        }
    }
}
