
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    // =========================
    // ENUMS
    public enum CharacterState
    {
        Idle,
        Walking,
        Running,
        Jumping
    }

    public CharacterState currentState;
    public enum currentHandState
    {
        Empty,
        HoldingBeanSeed,
        HoldingBeetrootSeed,
        HoldingBroccoliSeed,
        HoldingCabbageSeed,
        HoldingCarrotSeed,
        HoldingChilliSeed,
        HoldingCornSeed,
        HoldingPepperSeed,
        HoldingPumpkinSeed,
        HoldingTomatoSeed,
        HodingWatermelonSeed,
        HodingWheatSeed,
        HoldingBean,
        HodingBeetroot,
        HoldingBroccoli,
        HoldingCabbage,
        HoldingCarrot,
        HoldingChilli,
        HoldingCorn,
        HoldingPepper,
        HoldingPumpkin,
        HoldingTomato,
        HoldingWatermelon,
        HoldingWheat,
        HoldingRohu,
        HoldingBigTree,
        HoldingCrismasTree

    }
    public bool SetHandStateFromString(string stateName)
    {
        if (Enum.TryParse<currentHandState>(stateName, true, out currentHandState newState))
        {
            handState = newState;
            return true;
        }

        Debug.LogWarning($"Invalid state name: '{stateName}'");
        return false;
    }
    public currentHandState handState;
    // =========================
    // MOVEMENT SETTINGS
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public Joystick joystick;
    private RaycastDetector raycast;
    private CharacterController controller;
    private Vector3 velocity;
    // Run Button State
    public static bool isRunPressed = false;
    [SerializeField] private Button jumpButton;
    // Character States
    [Header("Character animation")]
    private Animator anim;
    [Header("Screen Rotation Settings")]
    public float rotationSpeed = 0.2f; // Touch sensitivity


    // Touch rotation
    private Vector2 lastTouchPosition;
    private bool isTouching = false;
    
    public static CharacterMovement instance;

    // mouse settings

    // =========================
    // PC CONTROL VARIABLES
    float mouseSensitivity = 3f;
    bool mouseLocked = true;
    public GameObject[] uiObjs;

    
    void mouseLockUnlock()
    {
        bool isUnLock = false;
        for (int i = 0; i < uiObjs.Length; i++)
        {
            if (uiObjs[i].activeSelf)
            {
                isUnLock = true;
               
            }
        }
        if (isUnLock)
        {
            UnlockMouse();
            
        }
        else
        {
            LockMouse();
        }
    }

    
    IEnumerator RunOnlyOnce()
    {
        Debug.Log("This runs ONLY once, even after restarting the game!");
        yield return new WaitForSeconds(1f);
        PlayerSaveManager.Instance.AddCoins(1000);
        PlayerSaveManager.Instance.AddItem("BeanSeed",5);
         PlayerSaveManager.Instance.AddItem("BeetrootSeed", 5);
          PlayerSaveManager.Instance.AddItem("BroccoliSeed", 5);
           PlayerSaveManager.Instance.AddItem("CabbageSeed", 5);
            PlayerSaveManager.Instance.AddItem("CarrotSeed", 5);
             PlayerSaveManager.Instance.AddItem("ChilliSeed", 5);
              PlayerSaveManager.Instance.AddItem("CornSeed", 5);
               PlayerSaveManager.Instance.AddItem("PepperSeed", 5);
                PlayerSaveManager.Instance.AddItem("PumpkinSeed", 5);
                 PlayerSaveManager.Instance.AddItem("TomatoSeed", 5);
                  PlayerSaveManager.Instance.AddItem("WatermelonSeed", 5);
                   PlayerSaveManager.Instance.AddItem("WheatSeed", 5);
        // Your code here
    }
    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseLocked = true;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mouseLocked = false;
    }
    // =========================
    // PC INPUTS
    // =========================
    void HandlePCInputs()
    {
        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }

        // Run
        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
        {
            isRunPressed = true;
        }
        else
        {
            isRunPressed = false;
        }

    }



    // =========================
    // MOUSE LOOK
    // =========================
    void HandleMouseLook()
    {
        if (!mouseLocked) return;
        if (Mouse.current == null) return;

        float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
    }

    // movement sound
    [Header("Footstep Settings")]
    public float walkStepRate = 0.5f;  // Time between walk footsteps
    public float runStepRate = 0.35f;  // Time between run footsteps
    private float stepTimer = 0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        currentState = CharacterState.Idle;
        jumpButton.onClick.AddListener(Jump);
        handState = currentHandState.Empty;
        instance = this;
        raycast=GetComponent<RaycastDetector>();
        if (PlayerPrefs.GetInt("HasRunBefore", 0) == 0)
        {
            StartCoroutine(RunOnlyOnce());

            PlayerPrefs.SetInt("HasRunBefore", 1);
            PlayerPrefs.Save();
        }
        LockMouse();
    }
    
    void Update()
    {
        HandleMovement();
        printMoveMentState();
        HandleGravity();
        HandleTouchRotation();
        if(currentState== CharacterState.Idle)
        FacePlayerToCamera();
        soundManagement();

        HandleMouseLook();
        HandlePCInputs();
        mouseLockUnlock();
    }
    bool x = false;
    void soundManagement()
    {

        if (GroundCheck.isGrounded) { 
            if (currentState == CharacterState.Walking)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                AudioManager.Instance.PlayWalkSound();
                stepTimer = walkStepRate;
            }
        }
        else if (currentState == CharacterState.Running)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                AudioManager.Instance.PlayRunSound();
                   
                stepTimer = runStepRate;
            }
        }
        else
        {
            // Reset timer when idle or jumping
            stepTimer = 0f;
        }
        }
    }
    void HandleTouchRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isTouching = true;
            }
            else if (touch.phase == UnityEngine.TouchPhase.Moved && isTouching)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                float yaw = delta.x * rotationSpeed;

                // Rotate player
                transform.Rotate(Vector3.up, yaw);

                lastTouchPosition = touch.position;
            }
            else if (touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
            {
                isTouching = false;
            }
        }

    }
    public void FacePlayerToCamera()
    {
        if (mainCam == null) return;

        // Get camera forward direction
        Vector3 direction = mainCam.transform.forward;

        // Ignore vertical tilt
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        // Rotate player toward camera direction
        this.gameObject.transform.rotation = Quaternion.LookRotation(direction);
    }
    // =========================
    // MOVEMENT
    // =========================
    //void HandleMovement()
    //{
    //    float x = joystick.Horizontal;
    //    float z = joystick.Vertical;

    //    float magnitude = new Vector2(x, z).magnitude;

    //    // IDLE
    //    if (magnitude < 0.2f && controller.isGrounded)
    //    {
    //        currentState = CharacterState.Idle;
    //        return;
    //    }

    //    // WALK / RUN
    //    float speed = isRunPressed ? runSpeed : walkSpeed;

    //    Debug.Log("Current speed: " + speed);
    //    Vector3 move = new Vector3(x, 0f, z);

    //    if (move.sqrMagnitude < 0.0001f)
    //    {
    //        currentState = CharacterState.Idle;
    //    }
    //    else
    //    {
    //                   currentState = isRunPressed ? CharacterState.Running : CharacterState.Walking;
    //    }
    //    controller.Move(move * speed * Time.deltaTime);
    //}
    public Camera mainCam;
    void HandleMovement()
    {
        //float x = joystick.Horizontal;
        //float z = joystick.Vertical;
        // Mobile joystick input
        float keyboardX = 0f;
        float keyboardZ = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            keyboardX = -1;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            keyboardX = 1;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            keyboardZ = 1;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            keyboardZ = -1;
        float x = joystick.Horizontal + keyboardX;
        float z = joystick.Vertical + keyboardZ;


        //added end
        float magnitude = new Vector2(x, z).magnitude;

        // IDLE
        if (magnitude < 0.2f && controller.isGrounded)
        {
            currentState = CharacterState.Idle;
            return;
        }

        // WALK / RUN
        float speed = isRunPressed ? runSpeed : walkSpeed;
        Vector3 move= new Vector3(x, 0f, z);
        // ---- CAMERA-RELATIVE MOVEMENT ----
        if (magnitude >= 0.2f)
        {
            // Get camera forward/right (ignore camera Y rotation for horizontal movement)
            Vector3 camForward = mainCam.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = mainCam.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            currentState = isRunPressed ? CharacterState.Running : CharacterState.Walking;
            // Calculate movement relative to camera
            move = camForward * z + camRight * x;

            // Move the character
            controller.Move(move * speed * Time.deltaTime);

            // Rotate player to face movement direction
            transform.forward = move.normalized;
        }
        else {             currentState = CharacterState.Idle; }
    }
    public void freezePlayer()
    {
        walkSpeed = 0;
        runSpeed = 0;
        jumpForce = 0;
        gravity = 0;
    }
    public void unfreezePlayer()
    {
               walkSpeed = 3f;
        runSpeed = 6f;
        jumpForce = 5f;
        gravity = -9.81f;
    }
    // =========================
    // GRAVITY
    // =========================

    public string getCurrentHandState()
    {
        return handState.ToString();
    }   
    void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // =========================
    // JUMP (Button)
    // =========================
    public void Jump()
    {
        if (!GroundCheck.isGrounded)
        {
            NoticeUI.Instance.ShowNotice("You can't jump while in the air!");
            return;



        }

        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        currentState = CharacterState.Jumping;
        anim.Play("Jump");
        StartCoroutine(wait());

    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(01f);
        AudioManager.Instance.playJumpSound();

    }
    // =========================
    // RUN BUTTON EVENTS
    // =========================
    public void StartRun()
    {
        isRunPressed = true;
    }

    public void StopRun()
    {
        isRunPressed = false;
    }
    private void printMoveMentState()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        // Dead zone to avoid noise
        float deadZone = 0.2f;

        if(currentState==CharacterState.Idle)
        {
            SetAllAnimFalse();
            if (Mathf.Abs(x) < deadZone && Mathf.Abs(y) < deadZone)
            {
              //  Debug.Log("Idle");
                anim.SetBool("Idle", true);
                return;
            }
        }

        if (currentState == CharacterState.Walking)
        {
            SetAllAnimFalse();
            //anim.SetBool("Walk Right", true);
            anim.SetBool("Walk Forward", true);
            //Debug.Log("Walking Right");
            //if (Mathf.Abs(x) > Mathf.Abs(y))
            //{
            //    if (x > 0)
            //    {
            //        SetAllAnimFalse();
            //        //anim.SetBool("Walk Right", true);
            //        anim.SetBool("Walk Forward", true);
            //        Debug.Log("Walking Right");
            //    }
            //    else
            //    {
            //        SetAllAnimFalse();
            //        Debug.Log("Walking Left");
            //        //anim.SetBool("Walk Left", true);
            //        anim.SetBool("Walk Forward", true);

            //    }
            //}
            //else
            //{
            //    if (y > 0)
            //    {
            //        Debug.Log("Walking Forward");
            //        SetAllAnimFalse();
            //        anim.SetBool("Walk Forward", true);
            //    }
            //    else
            //    {
            //        Debug.Log("Walking Backward");
            //        SetAllAnimFalse();
            //        //anim.SetBool("Walk Backward", true);
            //        anim.SetBool("Walk Forward", true);
            //    }
            //}
        }else if (currentState == CharacterState.Running)
        {
            SetAllAnimFalse();
            anim.SetBool("Run", true);

            //if (Mathf.Abs(x) > Mathf.Abs(y))
            //{
            //    if (x > 0)
            //    {
            //        SetAllAnimFalse();
            //        anim.SetBool("Run", true);
            //        Debug.Log("Run Right");
            //    }
            //    else
            //    {
            //        SetAllAnimFalse();
            //        Debug.Log("Run Left");
            //        anim.SetBool("Run", true);

            //    }
            //}
            //else
            //{
            //    if (y > 0)
            //    {
            //        Debug.Log("Run Forward");
            //        SetAllAnimFalse();
            //        anim.SetBool("Run Forward", true);
            //    }
            //    else
            //    {
            //        Debug.Log("Run Backward");
            //        SetAllAnimFalse();
            //        anim.SetBool("Run Backward", true);
            //    }
            //}
        }


    }
    // =========================
    // DEBUG STATE
    // =========================
    private void SetAllAnimFalse()
    {
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Idle", false);
       // anim.SetBool("Jump", false);
        anim.SetBool("Run", false);
    }
    public void makeHandStateEmpty()
    {
        handState = currentHandState.Empty;
    }

}
