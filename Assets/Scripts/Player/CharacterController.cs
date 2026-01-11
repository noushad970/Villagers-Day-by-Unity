
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    public Joystick joystick;

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
    public enum CharacterState
    {
        Idle,
        Walking,
        Running,
        Jumping
    }

    public CharacterState currentState;
    private CharacterState lastState;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        currentState = CharacterState.Idle;
        jumpButton.onClick.AddListener(Jump);
    }

    void Update()
    {
        HandleMovement();
        printMoveMentState();
        HandleGravity();
        HandleTouchRotation();
    }

    void HandleTouchRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                float yaw = delta.x * rotationSpeed;

                // Rotate player
                transform.Rotate(Vector3.up, yaw);

                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
            }
        }
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
        float x = joystick.Horizontal;
        float z = joystick.Vertical;

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


    // =========================
    // GRAVITY
    // =========================
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
            return;


        }

        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        currentState = CharacterState.Jumping;
        anim.Play("Jump");
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
                Debug.Log("Idle");
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
            Debug.Log("Run Right");

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
        anim.SetBool("Walk Right", false);
        anim.SetBool("Walk Left", false);
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Walk Backward", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Run", false);
    }


}
