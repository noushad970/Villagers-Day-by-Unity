using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
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
        HoldingPumkinSeed,
        HoldingTomatoSeed,
        HoldingWatermelonSeed,
        HoldingWheatSeed,
        HoldingBean,
        HoldingBeetroot,
        HoldingBroccoli,
        HoldingCabbage,
        HoldingCarrot,
        HoldingChilli,
        HoldingCorn,
        HoldingPepper,
        HoldingPumkin,
        HoldingTomato,
        HoldingWatermelon,
        HoldingWheat,
        HoldingRohu,
        HoldingBigTree,
        HoldingCrismasTree
    }

    public currentHandState handState;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    public Joystick joystick;
    private CharacterController controller;
    private Vector3 velocity;

    public static bool isRunPressed = false;
    [SerializeField] private Button jumpButton;

    private Animator anim;
    private const string RUN_KEY = "HasRunBefore2";

    

    IEnumerator RunOnlyOnce(System.Action functionToRun)
    {
        yield return new WaitForSeconds(1f); // Delay to ensure everything is initialized
        if (!PlayerPrefs.HasKey(RUN_KEY))
        {
            functionToRun.Invoke(); // Run your function

            PlayerPrefs.SetInt(RUN_KEY, 1); // Mark as executed
            PlayerPrefs.Save();
        }
    }

    void MyFunction()
    { // Optional delay before running the function
        Debug.Log("This runs ONLY once, even after restarting the game.");

        PlayerSaveManager.Instance.AddCoins(1000);
        PlayerSaveManager.Instance.AddItem("BeanSeed", 5);
        PlayerSaveManager.Instance.AddItem("CarrotSeed", 5);
        PlayerSaveManager.Instance.AddItem("TomatoSeed", 5);
        PlayerSaveManager.Instance.AddItem("WheatSeed", 5);
        PlayerSaveManager.Instance.AddItem("BigTree", 5);
    }
    [Header("Camera")]
    public Camera mainCam;
    public float rotationSpeed = 0.2f;

    private Vector2 lastTouchPosition;
    private bool isTouching = false;

    public static CharacterMovement instance;

    float mouseSensitivity = 3f;
    bool mouseLocked = true;
    public GameObject[] uiObjs;

    [Header("Footstep Settings")]
    public float walkStepRate = 0.5f;
    public float runStepRate = 0.35f;
    private float stepTimer = 0f;
    public GameObject tutorialPanel;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        defaultWalkSpeed = walkSpeed;
        defaultRunSpeed = runSpeed;
        defaultJumpForce = jumpForce;
        defaultGravity = gravity;
        currentState = CharacterState.Idle;
        jumpButton.onClick.AddListener(Jump);
        handState = currentHandState.Empty;

        StartCoroutine(RunOnlyOnce(MyFunction));
        instance = this;

    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
        HandleTouchRotation();

        RotatePlayerToCamera(); // ✅ NEW (smooth follow camera)

        printMoveMentState();
        soundManagement();

        HandleMouseLook();
        HandlePCInputs();
        mouseLockUnlock();
    }

    // ================= PC INPUT =================
    void HandlePCInputs()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            Jump();

        isRunPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
    }

    void HandleMouseLook()
    {
        if (!mouseLocked || Mouse.current == null) return;

        float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    // ================= MOBILE CAMERA ROTATION =================
    void HandleTouchRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Ignore UI touches (joystick etc.)
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == UnityEngine.TouchPhase.Moved)
            {
                float yaw = touch.deltaPosition.x * rotationSpeed;

                // Rotate camera around player
                mainCam.transform.RotateAround(transform.position, Vector3.up, yaw);
            }
        }
    }

    // ================= PLAYER FOLLOW CAMERA =================
    void RotatePlayerToCamera()
    {
        if (mainCam == null) return;

        Vector3 direction = mainCam.transform.forward;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    // ================= MOVEMENT =================
    void HandleMovement()
    {
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

        float magnitude = new Vector2(x, z).magnitude;

        Vector3 camForward = mainCam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = mainCam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = camForward * z + camRight * x;
        move = Vector3.ClampMagnitude(move, 1f);

        if (magnitude >= 0.2f)
        {
            float baseSpeed = isRunPressed ? runSpeed : walkSpeed;

            // Direction-based speed
            float speedMultiplier = 1f;

            if (z < 0)
                speedMultiplier = 0.5f;
            else if (Mathf.Abs(x) > 0.1f && z <= 0.1f)
                speedMultiplier = 0.5f;
            else if (Mathf.Abs(x) > 0.1f && z > 0.1f)
                speedMultiplier = 0.75f;

            float finalSpeed = baseSpeed * speedMultiplier;

            controller.Move(move * finalSpeed * Time.deltaTime);

            // ✅ SMOOTH ROTATION TOWARD MOVEMENT
            if (move.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    10f * Time.deltaTime   // 🔥 adjust for smoothness
                );
            }

            currentState = isRunPressed ? CharacterState.Running : CharacterState.Walking;
        }
        else
        {
            currentState = CharacterState.Idle;
        }
    }

    // ================= GRAVITY =================
    void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ================= JUMP =================
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
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.playJumpSound();
    }

    // ================= SOUND =================
    void soundManagement()
    {
        if (!GroundCheck.isGrounded) return;

        stepTimer -= Time.deltaTime;

        if (currentState == CharacterState.Walking && stepTimer <= 0f)
        {
            AudioManager.Instance.PlayWalkSound();
            stepTimer = walkStepRate;
        }
        else if (currentState == CharacterState.Running && stepTimer <= 0f)
        {
            AudioManager.Instance.PlayRunSound();
            stepTimer = runStepRate;
        }
    }

    // ================= ANIMATION =================
    private void printMoveMentState()
    {
        SetAllAnimFalse();

        if (currentState == CharacterState.Idle)
            anim.SetBool("Idle", true);
        else if (currentState == CharacterState.Walking)
            anim.SetBool("Walk Forward", true);
        else if (currentState == CharacterState.Running)
            anim.SetBool("Run", true);
    }

    private void SetAllAnimFalse()
    {
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);
    }

    // ================= MOUSE LOCK =================
    void mouseLockUnlock()
    {
        bool unlock = false;

        foreach (var obj in uiObjs)
            if (obj.activeSelf || tutorialPanel.activeSelf) unlock = true;

        if (unlock) UnlockMouse();
        else LockMouse();
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

    // ================= HAND STATE =================
    public bool SetHandStateFromString(string stateName)
    {
        if (Enum.TryParse(stateName, true, out currentHandState newState))
        {
            handState = newState;
            return true;
        }

        Debug.LogWarning($"Invalid state name: '{stateName}'");
        return false;
    }

    public void makeHandStateEmpty()
    {
        handState = currentHandState.Empty;
    }


    // ================= FREEZE SYSTEM =================

    // Add these variables at the top of your script (outside functions):
    // float defaultWalkSpeed;
    // float defaultRunSpeed;
    // float defaultJumpForce;
    // float defaultGravity;

    // And initialize them inside Start():
    // defaultWalkSpeed = walkSpeed;
    // defaultRunSpeed = runSpeed;
    // defaultJumpForce = jumpForce;
    // defaultGravity = gravity;

    public void freezePlayer()
    {
        walkSpeed = 0f;
        runSpeed = 0f;
        jumpForce = 0f;
        gravity = 0f;

        // Optional (strong freeze)
        // controller.enabled = false;
    }
    float defaultWalkSpeed;
    float defaultRunSpeed;
    float defaultJumpForce;
    float defaultGravity;
    public void unfreezePlayer()
    {
        walkSpeed = defaultWalkSpeed;
        runSpeed = defaultRunSpeed;
        jumpForce = defaultJumpForce;
        gravity = defaultGravity;

        // Optional (if you disabled controller)
        // controller.enabled = true;
    }
}