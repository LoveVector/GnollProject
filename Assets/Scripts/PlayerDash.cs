using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public bool isDashing;

    public float dashSpeed;
    public float dashCooldown;
    public float timeForNextDash;
    public int totalDash;

    [SerializeField]float timeLeftCooldown;
    [SerializeField]int dashAttempts;
    [SerializeField]float dashStartTime;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleDash();
        DashCooldown();
    }

    void HandleDash()
    {
        bool isTryingToDash = Input.GetKeyDown(KeyCode.LeftShift);

        if (isTryingToDash && !isDashing)
        {
            if (dashAttempts <= totalDash)
            {
                OnStartDash();
            }
        }

        if(isDashing)
        {
            if (Time.time - dashStartTime <= timeForNextDash)
            {
                if (playerController.rb.velocity.Equals(Vector3.zero))
                {
                    //Player is not giving any input, just dash forward
                    playerController.rb.AddForce(transform.forward * (dashSpeed * 2), ForceMode.Force);
                    Debug.Log("Forward Dash");  
                }
                else
                {
                    playerController.rb.AddForce(playerController.rb.velocity.normalized * dashSpeed, ForceMode.Force);
                    Debug.Log("Directional Dash");
                }
            }
            else
            {
                OnEndDash();
            }
        }
    }

    void OnStartDash()
    {
        isDashing = true;
        dashStartTime = Time.time;
        dashAttempts += 1;
    }
    void OnEndDash()
    {
        isDashing = false;
        timeLeftCooldown = dashCooldown;
        dashStartTime = 0;
    }

    void DashCooldown()
    {
        if(timeLeftCooldown >= 0 )
        {
            timeLeftCooldown -= Time.deltaTime;
        }
        if(dashAttempts == 3)
        {
            if(timeLeftCooldown <= 0)
            {
                dashAttempts = 0;
            }
        }
    }

}
