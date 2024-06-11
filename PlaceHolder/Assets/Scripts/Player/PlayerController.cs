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

    [HideInInspector]
    public float moveSpeed;
    
    [HideInInspector]
    public float boostMoveSpeed;

    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode runKey = KeyCode.LeftShift;
    private KeyCode crouchKey = KeyCode.LeftControl;



    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    float stamina;
    float hp;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        hp = 100f;
        stamina = 100f;
        boostMoveSpeed = 1.75f;
        staminaRedZone = 30f;

        readyToJump = true;
        running = false;
        penaltyStamina = false;
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

        MyInput();
        if (running)
        {
            stamina -= 0.03f;
            moveSpeed = baseMoveSpeed * boostMoveSpeed;
        }
        else
        {
            if (stamina <= 100f)
                stamina += 0.01f;
            moveSpeed = baseMoveSpeed;
        }
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(runKey) && grounded)
        {
            if (stamina >= staminaRedZone){
                penaltyStamina = false;
                Run(true);
            }
            else if (stamina > 0f){
                if(!penaltyStamina)
                    Run(true);
            }
            else if(stamina < 0.6f){
                penaltyStamina = true;
                Run(false);
            }
            else{
                Run(false);
            }
        }
        else{
            Run(false);
        }

        if (Input.GetKey(crouchKey) && grounded)
            Crouch(true);
        else
            Crouch(false);


        //isCrouch();
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else if (!grounded)
            rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * airMultiplier,
                ForceMode.Force
            );
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
        if(typeRun)
            running = true;
        else
            running = false;
    }

    private void Crouch(bool typeCrouch){
        if (typeCrouch){
            rb.transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        else{
            rb.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
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

}
