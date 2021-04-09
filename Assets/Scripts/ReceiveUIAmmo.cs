using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveUIAmmo : MonoBehaviour
{
    Shooting shootAmmo;
    Text ammoText;
    // Start is called before the first frame update
    void Start()
    {
        shootAmmo = GetComponentInParent<Shooting>();
        ammoText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = shootAmmo.currentAmmo.ToString();
    }
}
