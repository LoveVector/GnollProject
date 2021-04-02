using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Camera playerCamera;

    public Rigidbody rb;

    public float cameraReducedHeight;
    public float feetReducedHeight;
    public float moveSpeed = 1f; 
    public float maxSpeed = 10f;
    public float mouseSensitivity = 1f;
    public float slideSpeed = 10f;
    public float jumpForce = 10f;
    public float wallJumpStrength;
    public float wallJumpHeightGain;
    public float slideVSlowDownTime = 1;
    public float slideDrag;
    public float slideAngularDrag;
    public float alreadySlidModifier = 2;
    public float fallSpeedThreshold = -7;
    public float fallingMass = 3;
    public float slideUnlocked = 1;

    public Vector3 bodyReducedSize;

    [SerializeField] bool isGrounded;
    [SerializeField] bool isOnWall;
    [SerializeField] bool alreadySlid;

    [SerializeField]GameObject stepRayHigh;
    [SerializeField]GameObject stepRayLow;

    [SerializeField]float stepHeight = 0.3f; // How high we want the character to climb when he steps
    [SerializeField]float stepSmooth = 0.1f;
    [SerializeField]float originalMass;
    [SerializeField]float originalAngularDrag;
    [SerializeField]float originalDrag;
    [SerializeField]float originalFeetHeight;
    [SerializeField]float stepRayLowDistance = 0.1f;
    [SerializeField]float stepRayHighDistance = 0.2f;
    [SerializeField]float alreadySlidTime;
    [SerializeField]float slideTime;

    private float jumpVelocity;

    CapsuleCollider feetCollider;
    BoxCollider bodyCollider;

    [SerializeField]Vector3 originalBodyHeight;
    [SerializeField]Vector3 originalCameraHeight;

    Vector3 lastVelocity;
    Vector3 moveInput;
    Vector2 mouseInput;

    private void Awake()
    {
        instance = this;                                    // Singleton

        rb = GetComponent<Rigidbody>();

        stepRayHigh = GameObject.Find("StepRayUpper");
        stepRayLow = GameObject.Find("StepRayLower");

        playerCamera = GetComponentInChildren<Camera>();
        feetCollider = GetComponent<CapsuleCollider>();
        bodyCollider = GetComponentInChildren<BoxCollider>();

        stepRayHigh.transform.position = new Vector3(stepRayHigh.transform.position.x, stepHeight, stepRayHigh.transform.position.z); 
    }


    void Start()
    {
        // -------------------------------- Gaining Original Variables -----------------------
        originalMass = rb.mass;
        originalAngularDrag = rb.angularDrag;
        originalDrag = rb.drag;
        originalCameraHeight = playerCamera.transform.localPosition;
        originalFeetHeight = feetCollider.height;
        originalBodyHeight = bodyCollider.size;
    }

    void Update()
    {
        lastVelocity = rb.velocity;
        MouseAim();
        CheckIfGrounded();
        Jump();
        InputSlide();
        //Debug.Log(rb.velocity);
    }

    private void FixedUpdate()
    {
        Movement();
        StepClimb();
    }

    void MovementVelocity()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.forward * moveInput.z;
        Vector3 moveVertical = transform.right * moveInput.x;
        //Vector3 moveUp = transform.up * jumpForce;

        rb.velocity = (moveHorizontal + moveVertical) * moveSpeed;
    }

    void MovementRigidbody()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        rb.AddForce(transform.forward * moveInput.z * moveSpeed, ForceMode.Acceleration);
        rb.AddForce(transform.right * moveInput.x * moveSpeed, ForceMode.Acceleration);
    }

    void Movement()
    {
        Vector2 horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);
        if (horizontalMovement.magnitude > maxSpeed)
        {
            horizontalMovement = horizontalMovement.normalized * maxSpeed;
        }
        rb.velocity = new Vector3(horizontalMovement.x, rb.velocity.y, horizontalMovement.y);
        rb.AddRelativeForce(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);
    }

    void MouseAim()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y - -mouseInput.x, 
            transform.rotation.eulerAngles.z);

        playerCamera.transform.localRotation = Quaternion.Euler(playerCamera.transform.localRotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }

    void CheckIfGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if(!isGrounded && rb.velocity.y <= fallSpeedThreshold)
        {
            rb.drag = 0;
            rb.mass = fallingMass;
        }
        else
        {
            rb.mass = originalMass;
            rb.drag = originalDrag;
        }
    }

    void InputSlide()
    {
        if (alreadySlid == true)
        {
            alreadySlidTime += Time.deltaTime;
        }
        if(alreadySlidTime >= slideUnlocked)
        {
            alreadySlid = false;
            alreadySlidTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            Sliding();
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            slideTime += Time.deltaTime;
            if (slideTime >= slideVSlowDownTime )
            {
                Debug.Log("Dragging");
                rb.drag = slideDrag;
                rb.angularDrag = slideAngularDrag;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            GoUp();
    }

    void Sliding()
    {

        playerCamera.transform.localPosition = Vector3.up * cameraReducedHeight;
        feetCollider.height = feetReducedHeight;
        bodyCollider.size = bodyReducedSize;
        if (alreadySlid == true)
        {
            Debug.Log("SlideMod working");
            rb.AddForce(transform.forward * alreadySlidModifier, ForceMode.VelocityChange);
        }
        else
        {
            Debug.Log("NoSlideMod");
            rb.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
        }

        alreadySlid = true;
    }

    void GoUp()
    {
        slideTime = 0;
        rb.angularDrag = originalAngularDrag;
        rb.drag = originalDrag;
        playerCamera.transform.localPosition = originalCameraHeight;
        feetCollider.height = originalFeetHeight;
        bodyCollider.size = originalBodyHeight;
    }

    void StepClimb()
    {
        RaycastHit hitLowerF;
        if (Physics.Raycast(stepRayLow.transform.position, transform.TransformDirection(Vector3.forward), out hitLowerF, stepRayLowDistance))
        {
            RaycastHit hitUpperF;
            if (!Physics.Raycast(stepRayHigh.transform.position, transform.TransformDirection(Vector3.forward), out hitUpperF, stepRayHighDistance))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLowerR;
        if (Physics.Raycast(stepRayLow.transform.position, transform.TransformDirection(Vector3.right), out hitLowerR, stepRayLowDistance))
        {
            RaycastHit hitUpperR;
            if (!Physics.Raycast(stepRayHigh.transform.position, transform.TransformDirection(Vector3.right), out hitUpperR, stepRayHighDistance))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLowerL;
        if (Physics.Raycast(stepRayLow.transform.position, transform.TransformDirection(-1, 0, 0), out hitLowerL, stepRayLowDistance))
        {
            RaycastHit hitUpperL;
            if (!Physics.Raycast(stepRayHigh.transform.position, transform.TransformDirection(Vector3.forward), out hitUpperL, stepRayHighDistance))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLowerB;
        if (Physics.Raycast(stepRayLow.transform.position, transform.TransformDirection(0, 0, -1), out hitLowerB, stepRayLowDistance))
        {
            RaycastHit hitUpperB;
            if (!Physics.Raycast(stepRayHigh.transform.position, transform.TransformDirection(0, 0, -1), out hitUpperB, stepRayHighDistance))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Wall")
        {
            Debug.Log("You hit a Wall");
            if (Input.GetButton("Jump") && !isGrounded)
            {
                var speed = lastVelocity.magnitude;
                var direction = Vector3.Reflect(lastVelocity.normalized + new Vector3(0, wallJumpHeightGain, 0), hit.contacts[0].normal);

                rb.velocity = direction * Mathf.Max(speed, wallJumpStrength);
                Debug.Log("Wall Jumped");
            }
        }
    }
}
