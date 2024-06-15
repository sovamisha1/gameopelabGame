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
    bool miniGameActive;
    bool isInteract;
    bool moveAndStep;
    bool eventComplite;

    [HideInInspector]
    public float moveSpeed;

    [HideInInspector]
    public float coefRunSpeed;

    [HideInInspector]
    public float coefCrouchSpeed;
    private float speedUpDown;

    private KeyCode jumpKey; // = InputManager.instance.GetKeyForAction("Jump"); // KeyCode.Space;
    private KeyCode runKey; //=  InputManager.instance.GetKeyForAction("Run"); //KeyCode.LeftShift;
    private KeyCode crouchKey; //= InputManager.instance.GetKeyForAction("Crouch"); //KeyCode.LeftControl;
    private KeyCode interactKey; //= InputManager.instance.GetKeyForAction("Interact"); //KeyCode.LeftControl;
    private KeyCode oneKey; //= InputManager.instance.GetKeyForAction("Interact1"); //KeyCode.LeftControl;
    private KeyCode twoKey; // = InputManager.instance.GetKeyForAction("Interact2"); //KeyCode.LeftControl;
    private KeyCode useKey; //= InputManager.instance.GetKeyForAction("UseItem"); //KeyCode.LeftControl;
    private KeyCode refilKey; //  = InputManager.instance.GetKeyForAction(RefilPotion); 

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    public LayerMask StairsStep;
    public LayerMask Ladder;
    bool grounded;
    bool stairssteped;
    bool isLadder;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    float stamina;
    float hp;
    float playerTakeRange;
    float inHand; //0 - ничего, 1 - Posion, 2- Staff

    Vector3 moveDirection;
    RaycastHit hit;
    AudioSource audioSource;
    

    public Camera cameraVector;
    private Inventory inventory;
    private Potion potion;
    private MagicStaff magicStaff;
    private Transform spawnPoint;
    private GameObject cameraPoint;
    public AudioClip fiCraftTable;
    public AudioClip pickUpBranch;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        hp = 100f;
        stamina = 100f;
        coefRunSpeed = 2f;
        coefCrouchSpeed = 0.7f;
        staminaRedZone = 30f;
        playerTakeRange = playerHeight * 1.25f;
        inHand = 0f;
        speedUpDown = 0.02f;
        
        

        jumpKey = InputManager.instance.GetKeyForAction("Jump");
        runKey = InputManager.instance.GetKeyForAction("Run");
        crouchKey = InputManager.instance.GetKeyForAction("Crouch");
        interactKey = InputManager.instance.GetKeyForAction("Interact");
        oneKey = InputManager.instance.GetKeyForAction("Interact1"); //KeyCode.LeftControl;
        twoKey = InputManager.instance.GetKeyForAction("Interact2");
        useKey = InputManager.instance.GetKeyForAction("UseItem");
        refilKey = InputManager.instance.GetKeyForAction("RefilPotion"); 
        

        if (cameraVector == null)
            cameraVector = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        if (orientation == null)
            orientation = GameObject.Find("Orientation").GetComponent<Transform>();
        if (spawnPoint == null)
            spawnPoint = GameObject.Find("SavePoint").GetComponent<Transform>();
        if (cameraPoint == null)
            cameraPoint = GameObject.Find("СameraPos"); 
        if (fiCraftTable == null)
            fiCraftTable = GetComponent<AudioClip>();
        if (pickUpBranch == null)
            pickUpBranch = GetComponent<AudioClip>();
        audioSource = GetComponent<AudioSource>();  


        //spawnPoint.position = 

        inventory = FindObjectOfType<Inventory>();
        potion = FindObjectOfType<Potion>();
        magicStaff = FindObjectOfType<MagicStaff>();

        dead = false;
        crouchActiv = false;
        readyToJump = true;
        running = false;
        penaltyStamina = false;
        isInteract = false;
        eventComplite = false;
        miniGameActive = false;
        isLadder = false;
        //moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        //_AdminKill();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, Ground);
        stairssteped = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, StairsStep);
        moveAndStep = (
            !(
                Input.GetKey(KeyCode.W)
                || Input.GetKey(KeyCode.UpArrow)
                || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.S)
                || Input.GetKey(KeyCode.DownArrow)
                || Input.GetKey(KeyCode.RightArrow)
                || Input.GetKey(KeyCode.LeftArrow)
            ) && stairssteped
        );
        if(!miniGameActive)
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
        else if (moveAndStep)
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
        if(dead)
            Death();
        ItemsManager();
        UseItems();
        ShowHint();
        GetInfoItems();
        UseLadder();
        
        //Debug.Log(grounded);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && !crouchActiv)
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

    }

    //=====БЛОК ПЕРЕМЕЩЕНИЯ ПЕРСОНАЖА====

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    

        // on ground
        if (grounded || stairssteped)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // in air
        else
        {
            rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * airMultiplier,
                ForceMode.Force
            );
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
        ); //
        rb.transform.localScale = new Vector3(3f, 1.5f, 3f);
        crouchActiv = true;
    }

    private void StandUp()
    {
        rb.transform.position = new Vector3(
            rb.transform.position.x,
            rb.transform.position.y + 0.5f,
            rb.transform.position.z
        );
        rb.transform.localScale = new Vector3(3f, 3f, 3f);
        crouchActiv = false;
    }

    private void UseLadder()
    {
        isLadder = Physics.Raycast(
            new Ray(cameraVector.transform.position, orientation.transform.forward),
            out hit,
            0.75f,
            Ladder
        );

        if (isLadder && Input.GetKey(interactKey)){
            rb.transform.position += Vector3.up * speedUpDown;
        }
    }

    
    //=====БЛОК ПАРАМЕТРЫ ПЕРСОНАЖА====

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
        if (hp > 100)
            hp = 100;
    }

    public void ReceivingDamage(float giveDamage)
    {
        hp -= giveDamage;
        if (hp < 0)
            dead = true;
    }

    //=====БЛОК ВЗАИМОДЕЙСТВИЯ С ПРЕДМЕТАМИ====

    public bool ShowHint()
    {
        int layerMask = LayerMask.GetMask("Interactable");
        isInteract = Physics.Raycast(
            new Ray(cameraVector.transform.position, cameraVector.transform.forward),
            out hit,
            playerTakeRange,
            layerMask
        );
        if (isInteract)
            return true;
        else
            return false;
    }

    private void GetInfoItems()
    {
        int layerMask = LayerMask.GetMask("Interactable");
        isInteract = Physics.Raycast(
            new Ray(cameraVector.transform.position, cameraVector.transform.forward),
            out hit,
            playerTakeRange,
            layerMask
        );
        if (isInteract && Input.GetKeyDown(interactKey))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
                eventComplite = interactable.IsImportant();
                if (eventComplite){
                    spawnPoint.position = rb.transform.position;
                }
                eventComplite = false;

            }
        }
    }

    private void ItemsManager(){
        if (Input.GetKeyDown(oneKey) && inventory.DoesContainItem("Зелье Лечения")){
            if(inHand == 0){
                potion.SelectPotion(true);
                inHand = 1;
            }
            else if(inHand == 2){
                magicStaff.SelectStaff(false);
                potion.SelectPotion(true);
                inHand = 1;
            }
            else{
                potion.SelectPotion(false);
                inHand = 0;
            }
        }
        else if(Input.GetKeyDown(twoKey) && inventory.DoesContainItem("Посох Мага")){
            if(inHand == 0){
                magicStaff.SelectStaff(true);
                inHand = 2;
            }
            else if(inHand == 1){
                potion.SelectPotion(false);
                magicStaff.SelectStaff(true);
                inHand = 2;
            }
            else{
                magicStaff.SelectStaff(false);
                inHand = 0;
            }
        }  
    }

    private void UseItems(){
        if(Input.GetKeyDown(useKey)){
            if(inHand == 1){
                potion.TryToDrink();
            }
            else if(inHand == 2){
                magicStaff.TryToAttack();
            }
        }
    }

    private void _RefilPotion(){
        if(Input.GetKeyDown(refilKey) && inHand == 1){
            potion.RefilPotion();
        }
    }

    //=====БЛОК РАБОТЫ С САУНДОМ ПЕРСОНАЖА====

    public void PlaySound(AudioClip audio){
        audioSource.PlayOneShot(audio);
    }

    public void FirstInteractCraftTable(){
        audioSource.PlayOneShot(fiCraftTable);
    }

    public void PickUpBranch(){
        audioSource.PlayOneShot(pickUpBranch);
    }

    /*public void PickUpBranch(){
        audioSource.PlayOneShot(pickUpBranch);
    }*/


    //=====БЛОК ВМЕШАТЕЛЬСТВА В ЖИЗНЬ ПЕРСОНАЖА====

    /*private void _AdminKill(){
        if(Input.GetKeyDown(KeyCode.L)){
            //StopPlayer(true, spawnPoint.transform);
        }
    } */

    public void StopPlayer(bool playerMode, Camera eventCamera){
        if(playerMode){
            miniGameActive = true;
            cameraVector.enabled = false;
            eventCamera.enabled = true;

        }
        else{
            miniGameActive = false;
            eventCamera.enabled = false;
            cameraVector.enabled = true;
        }
    }

    public void Death(){

        StandUp();
        rb.transform.position = spawnPoint.position;
        

        hp = 100f;
        stamina = 100f;

        dead = false;
        crouchActiv = false;
        readyToJump = true;
        running = false;
        penaltyStamina = false;
        isInteract = false;
        eventComplite = false;
        miniGameActive = false;
        isLadder = false;
    }
}
