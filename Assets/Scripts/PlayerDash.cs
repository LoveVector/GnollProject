using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    Camera playerCam;
    public bool isDashing;

    public float dashSpeed;
    public float dashCooldown;
    public float timeForNextDash;
    public int dashAttempts;
    public int totalDash;
    public float dashPrevFOV = 60;
    public float dashFOV = 92;
    public float originalDashFOVTime = 2;
    public float dashFOVTime = 2;
    public float fovSpeed = 1;

    public float originalFOV;

    [SerializeField]float timeLeftCooldown;
    [SerializeField]float dashStartTime;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GetComponentInChildren<Camera>();
        playerController = GetComponent<PlayerController>();

        originalFOV = playerCam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDash();
        DashCooldown();
        DashFOVTime();
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("This is happening");
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, dashFOV, fovSpeed * Time.deltaTime);
            dashFOVTime =- Time.deltaTime;
            if (dashFOVTime <= 0)
            {
                playerCam.fieldOfView = originalFOV;
            }
        }*/
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
        //playerCam.fieldOfView = dashFOV;
        Debug.Log("This is happening");
        playerCam.fieldOfView = Mathf.Lerp(dashPrevFOV, dashFOV, dashFOVTime * Time.time);
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
    void DashFOVTime()
    {

        if(playerCam.fieldOfView == dashFOV)
        {
            Debug.Log("DashFOVChanged");
            dashFOVTime -= Time.deltaTime;
            if(dashFOVTime <= 0)
            {
                playerCam.fieldOfView = originalFOV;
                dashFOVTime = originalDashFOVTime;
            }
        }
    }
}
