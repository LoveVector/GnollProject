using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveUIAmmo : MonoBehaviour
{
    Shooting shootAmmo;
    Text ammoText;
    int shotgunAmmoValue;

    public bool isShotgun;

    // Start is called before the first frame update
    void Start()
    {
        shootAmmo = FindObjectOfType<Shooting>();
        ammoText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shootAmmo.gameObject.activeInHierarchy == false)
        {
            Debug.Log("AmmoChanged");
            shootAmmo = FindObjectOfType<Shooting>();
        }
        if (shootAmmo.isShotgun)
        {
            shotgunAmmoValue = shootAmmo.currentAmmo / shootAmmo.bulletsShot;
            ammoText.text = shotgunAmmoValue.ToString();
        }
        else
        ammoText.text = shootAmmo.currentAmmo.ToString();
    }
}
