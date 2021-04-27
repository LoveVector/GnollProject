using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveHealthUI : MonoBehaviour
{
    Text healthText;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = player.currentHealth.ToString("F0") + "%";
    }
}
