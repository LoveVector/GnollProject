using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount;
    public int ammoMin = 10;
    public int ammoMax = 30;
    public Shooting shootingSc;

    private void OnTriggerEnter(Collider other)
    {
        shootingSc = other.GetComponentInChildren<Shooting>();
        if(other.tag == "Player")
        {
            ammoAmount = Random.Range(ammoMin, ammoMax);
            shootingSc.currentAmmo += ammoAmount;
            Destroy(gameObject);
        }
    }
}
