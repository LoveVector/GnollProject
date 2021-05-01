using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;
    public int healMin = 5;
    public int healMax = 15;
    public PlayerController playerSc;
    public Animator UIAnimate;
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        UIAnimate = GameObject.Find("HealVig").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerSc = FindObjectOfType<PlayerController>();
        if (other.tag == "Player")
        {
            audioManager.Play("Heal");
            UIAnimate.SetTrigger("getHealed");
            healAmount = Random.Range(healMin, healMax);
            playerSc.currentHealth += healAmount;
            Destroy(gameObject);
        }
    }
}
