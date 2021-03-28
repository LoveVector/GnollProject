using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    PlayerController player;
    public GameObject bulletImpact;

    public int currentAmmo = 50;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        Shoot();   
    }

    void Shoot()
    {

        if(Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Ray ray = player.playerCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //bug.Log("I'm looking at " + hit.transform.name);
                    Instantiate(bulletImpact, hit.point, transform.rotation);
                }
                else
                {
                    Debug.Log("I'm looking at nothing");
                }
                currentAmmo--;
            }

        }
    }
}
