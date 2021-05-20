using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileDamagePlayer : MonoBehaviour
{
    public GameObject playerGameObject;
    public EnemyMeleeAI enemyAIDamage;

    public float bulletSpeed;

    Vector3 playerposition;

    // Start is called before the first frame update
    private void Start()
    {
        enemyAIDamage = GetComponentInParent<EnemyMeleeAI>();
        playerGameObject = GameObject.Find("Player");
        Vector3 playerPos = playerGameObject.transform.position;

        // Aim bullet in player's direction.
        transform.LookAt(playerPos);
    }

    private void Update()
    {
        transform.Translate(transform.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerController>().TakeDamagePlayer(enemyAIDamage.attackDamageValue);
        }
        Destroy(gameObject);
    }
}
