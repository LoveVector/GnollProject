using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Animator gunAnim;
    public GameObject bulletImpact;

    public int damage;
    public int currentAmmo = 50;
    public int maxAmmo;
    public int bulletsShot;

    public float fireRate;
    public float spread;
    public float range;

    public bool allowButtonHold;

    public LayerMask isEnemy;

    bool readyToShoot;
    bool shooting;

    PlayerController player;

    [SerializeField] float fireRateTime;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        gunAnim = GetComponent<Animator>();
    }

    void Update()
    {
        MyInput();
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }

    void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        Shoot();
    }

    void Shoot()
    {
        if (Time.time > fireRateTime)
        {
            if (shooting && currentAmmo > 0)
            {
                for (int i = 0; i < bulletsShot; i++)
                {
                    float x = Random.Range(-spread, spread);
                    float y = Random.Range(-spread, spread);

                    Vector3 direction = player.playerCamera.transform.forward + new Vector3(x, y, 0);

                    //Ray ray = player.playerCamera.ViewportPointToRay(new Vector3(.5f * x, .5f * y, 0));
                    RaycastHit hit;

                    if (Physics.Raycast(player.playerCamera.transform.position, direction, out hit, range))
                    {

                        Instantiate(bulletImpact, hit.point, Quaternion.identity);
                        Debug.Log("I Shot something");
                        //bug.Log("I'm looking at " + hit.transform.name);
                        //Look for enemy script and damage him
                    }
                    else
                    {
                        Debug.Log("I Shot Nothing");
                    }
                    currentAmmo--;
                    gunAnim.SetTrigger("Shoot");
                    fireRateTime = Time.time + fireRate;
                }
            }
        }
    }
}