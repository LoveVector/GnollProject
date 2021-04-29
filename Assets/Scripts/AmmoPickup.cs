using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount;
    public int ammoMin = 10;
    public int ammoMax = 30;
    public Shooting shootingSc;
    public Animator AmmoUIAnim;

    private void Awake()
    {
        AmmoUIAnim = GameObject.Find("AmmoVig").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        shootingSc = other.GetComponentInChildren<Shooting>();
        if(other.tag == "Player")
        {
            AmmoUIAnim.SetTrigger("getAmmo");
            ammoAmount = Random.Range(ammoMin, ammoMax);
            shootingSc.currentAmmo += ammoAmount;
            Destroy(gameObject);
        }
    }
}
