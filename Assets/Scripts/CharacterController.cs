using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
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
    }

    // =========================
    // MOVEMENT
    // =========================
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
       
        Debug.Log("Current speed: " + speed);
        Vector3 move = new Vector3(x, 0f, z);
        if (move.sqrMagnitude < 0.0001f)
        {
            currentState = CharacterState.Idle;
        }
        else
        {
                       currentState = isRunPressed ? CharacterState.Running : CharacterState.Walking;
        }
        controller.Move(move * speed * Time.deltaTime);
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
            
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    SetAllAnimFalse();
                    anim.SetBool("Walk Right", true);
                    Debug.Log("Walking Right");
                }
                else
                {
                    SetAllAnimFalse();
                    Debug.Log("Walking Left");
                    anim.SetBool("Walk Left", true);
                    
                }
            }
            else
            {
                if (y > 0)
                {
                    Debug.Log("Walking Forward");
                    SetAllAnimFalse();
                    anim.SetBool("Walk Forward", true);
                }
                else
                {
                    Debug.Log("Walking Backward");
                    SetAllAnimFalse();
                    anim.SetBool("Walk Backward", true);
                }
            }
        }else if (currentState == CharacterState.Running)
        {

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    SetAllAnimFalse();
                    anim.SetBool("Run Right", true);
                    Debug.Log("Run Right");
                }
                else
                {
                    SetAllAnimFalse();
                    Debug.Log("Run Left");
                    anim.SetBool("Run Left", true);

                }
            }
            else
            {
                if (y > 0)
                {
                    Debug.Log("Run Forward");
                    SetAllAnimFalse();
                    anim.SetBool("Run Forward", true);
                }
                else
                {
                    Debug.Log("Run Backward");
                    SetAllAnimFalse();
                    anim.SetBool("Run Backward", true);
                }
            }
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
    }

}
