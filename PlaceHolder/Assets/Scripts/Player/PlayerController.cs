using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseMoveSpeed;

    //
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [HideInInspector]
    public float staminaRedZone;
    bool running;
    bool readyToJump;
    bool penaltyStamina;
    bool crouchActiv;
    bool dead;

    [HideInInspector]
    public float moveSpeed;

    [HideInInspector]
    public float coefRunSpeed;

    [HideInInspector]
    public float coefCrouchSpeed;

    private KeyCode jumpKey; // = InputManager.instance.GetKeyForAction("Jump"); // KeyCode.Space;
    private KeyCode runKey; //=  InputManager.instance.GetKeyForAction("Run"); //KeyCode.LeftShift;
    private KeyCode crouchKey; //= InputManager.instance.GetKeyForAction("Crouch"); //KeyCode.LeftControl;
    private KeyCode interactKey; //= InputManager.instance.GetKeyForAction("Interact"); //KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    public LayerMask StairsStep;
    bool grounded;
    bool stairssteped;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    float stamina;
    float hp;
    float playerTakeRange;

    Vector3 moveDirection;
    RaycastHit hit;
    public Camera cameraVector;

    Rigidbody rb;
    bool isInteract;
    bool moveAndStep;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        hp = 50f;
        stamina = 100f;
        coefRunSpeed = 1.75f;
        coefCrouchSpeed = 0.7f;
        staminaRedZone = 30f;
        playerTakeRange = playerHeight * 1.25f;

        jumpKey = InputManager.instance.GetKeyForAction("Jump");
        runKey = InputManager.instance.GetKeyForAction("Run");
        crouchKey = InputManager.instance.GetKeyForAction("Crouch");
        interactKey = InputManager.instance.GetKeyForAction("Interact");

        if (cameraVector == null)
            cameraVector = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        dead = false;
        crouchActiv = false;
        readyToJump = true;
        running = false;
        penaltyStamina = false;
        isInteract = false;
        //moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        
        // ground check
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.3f,
            Ground
        );
        stairssteped = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.3f,
            StairsStep
        );
        moveAndStep = (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)
         || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)
         || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) 
         && stairssteped);

        MyInput();
        if (running)
        {
            stamina -= 0.03f;
            moveSpeed = baseMoveSpeed * coefRunSpeed;
        }
        else if (crouchActiv)
        {
            if (stamina <= 100f)
                stamina += 0.01f;
            moveSpeed = baseMoveSpeed * coefCrouchSpeed;
        }
        else
        {
            if (stamina <= 100f)
                stamina += 0.01f;
            moveSpeed = baseMoveSpeed;
        }
        SpeedControl();
        //Debug.Log(moveAndStep);

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else if(moveAndStep)
            rb.drag = 999f;
            
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        ShowHint();
        GetInfoItems();
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded && !crouchActiv)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(runKey) && grounded && !crouchActiv)
        {
            if (stamina >= staminaRedZone)
            {
                penaltyStamina = false;
                Run(true);
            }
            else if (stamina > 0f)
            {
                if (!penaltyStamina)
                    Run(true);
            }
            else if (stamina < 0.6f)
            {
                penaltyStamina = true;
                Run(false);
            }
            else
            {
                Run(false);
            }
        }
        else
        {
            Run(false);
        }

        if (Input.GetKeyDown(crouchKey) && grounded && !crouchActiv)
            Crouch();
        else if (Input.GetKeyDown(crouchKey) && grounded && crouchActiv)
            StandUp();

        

        //isCrouch();
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded || stairssteped){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // in air
        else{
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier,ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Run(bool typeRun)
    {
        if (typeRun)
            running = true;
        else
            running = false;
    }

    private void Crouch()
    {
        if (running)
            Run(false);
        rb.transform.position = new Vector3(
            rb.transform.position.x,
            rb.transform.position.y - 0.5f,
            rb.transform.position.z
        );
        rb.transform.localScale = new Vector3(1f, 0.5f, 1f);
        crouchActiv = true;
    }

    private void StandUp()
    {
        rb.transform.position = new Vector3(
            rb.transform.position.x,
            rb.transform.position.y + 0.5f,
            rb.transform.position.z
        );
        rb.transform.localScale = new Vector3(1f, 1f, 1f);
        crouchActiv = false;
    }

    public float GetParametrs(string nameParametrs)
    {
        switch (nameParametrs)
        {
            case "stamina":
                return stamina;
            case "hp":
                return hp;
            default:
                return 0;
        }
    }

    public void UseHeal(float giveHp)
    {
        hp += giveHp;
        if (hp>100)
            hp = 100;
    }

    public void ReceivingDamage(float giveDamage)
    {
        hp -= giveDamage;
        if (hp<0)
            dead = true;
    }

    public bool ShowHint(){
        int layerMask = LayerMask.GetMask("Interactable");
        isInteract = Physics.Raycast(new Ray(cameraVector.transform.position, cameraVector.transform.forward), out hit, playerTakeRange, layerMask);
        if(isInteract)
            return true;
        else 
            return false;
    }

    private void GetInfoItems(){
        int layerMask = LayerMask.GetMask("Interactable");
        isInteract = Physics.Raycast(new Ray(cameraVector.transform.position, cameraVector.transform.forward), out hit, playerTakeRange, layerMask);
        if(isInteract && Input.GetKeyDown(interactKey)){
            Debug.Log(isInteract);
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null){
                interactable.Interact();
            }
        }
    }
    




}





