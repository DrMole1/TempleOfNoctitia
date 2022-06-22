using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSController : MonoBehaviour {

    // ======================== VARIABLES ========================
    
    [Header("Properties")]
    public float walkSpeed = 3;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public bool isOnWater = false;

    [Header("States")]
    public bool disabled;
    public bool camDisabled;
    public bool gravityDisabled;
    public bool canUsePowerAttract = false;
    public bool duringElevator = false;
    public bool canUsePowerGrab = false;

    // About Camera
    private Vector2 pitchMinMax = new Vector2 (-90, 85);
    private float rotationSmoothTime = 0.07f;
    private CharacterController controller;
    private Camera cam;
    [HideInInspector] public float yaw;
    [HideInInspector] public float pitch;
    [HideInInspector] public float smoothYaw;
    private float smoothPitch;
    private float yawSmoothV;
    private float pitchSmoothV;
    private float verticalVelocity;

    // About Movement
    private Vector3 velocity;
    private Vector3 smoothV;
    private bool jumping;
    private float lastGroundedTime;
    Vector2 input = new Vector2(0f, 0f);

    [Header("Component to drop")]
    public Animator animator;
    public Transform head;

    [Header("Sounds")]
    private SoundManager soundManager;
    public AudioSource playerAudio;
    private bool canStartSoundFootstep = true;
    private bool canStartSoundFall = true;
    private bool isStoppingWaterSound = false;

    [Header("Swimming")]
    public GameObject ptcWaterEffectPref01;
    public Transform PoolObjects;
    private Vector3 currentRotPtc01;
    private Quaternion currentQuaternionRotPtc01;

    // ===========================================================



    void Start () 
    {
        if (GameObject.Find("SoundManager"))
        {
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        cam = Camera.main;
        if (lockCursor) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController> ();

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
    }

    void Update () 
    {
        // Desactive player's inputs
        if (!disabled)
        {
            input = new Vector2(Input.GetAxisRaw("HorizontalAZERTY"), Input.GetAxisRaw("VerticalAZERTY"));

            Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
            Vector3 worldInputDir = transform.TransformDirection(inputDir);

            float currentSpeed = walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);
        }

        if (!gravityDisabled)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

            var flags = controller.Move(velocity * Time.deltaTime);
            if (flags == CollisionFlags.Below)
            {
                jumping = false;
                lastGroundedTime = Time.time;
                verticalVelocity = 0;
            }
        }

        if(!disabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
                if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f))
                {
                    jumping = true;
                    verticalVelocity = jumpForce;
                    canStartSoundFall = true;
                    canStartSoundFootstep = true;
                }
            }
        }

        if (!camDisabled)
        {
            float mX = 0;
            float mY = 0;

            mX = Input.GetAxisRaw("Mouse X");
            mY = Input.GetAxisRaw("Mouse Y");


            float mMag = Mathf.Sqrt(mX * mX + mY * mY);
            if (mMag > 5)
            {
                mX = 0;
                mY = 0;
            }

            yaw += mX * 2;
            pitch -= mY * 2;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
            smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

            transform.eulerAngles = Vector3.up * smoothYaw;
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            //head.localEulerAngles = Vector3.right * smoothPitch;
        }
    }
}