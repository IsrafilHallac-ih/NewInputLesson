using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    //------------------------------------NEW INPUT 2.YÖntem------------------------
    //Vector2 moveInput;
    //Vector2 mouseInput;
    //public void OnMove(InputValue value)
    //{
    //    moveInput = value.Get<Vector2>();
    //    Debug.Log(moveInput);
    //}

    //public void OnLook(InputValue value)
    //{
    //    moveInput = value.Get<Vector2>();
    //    Debug.Log(moveInput.x);
    //}

    //public void OnJump()
    //{
    //    Debug.Log("Oyuncu Zýpladý");
    //}



    //------------------------------------NEW INPUT 3.YÖntem------------------------
    //InputSystem_Actions ýnputActions;
    //Vector2 moveInput;

    //private void Awake()
    //{
    //    ýnputActions = new InputSystem_Actions();
    //}

    //private void OnEnable()
    //{
    //    ýnputActions.Player.Enable();
    //    //ýnputActions.Player.Move.performed += OnMovePerformed;
    //    //ýnputActions.Player.Move.canceled += OnMoveCanceled;

    //    ýnputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
    //    ýnputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    //}
    //private void OnDisable()
    //{
    //    ýnputActions.Player.Disable();
    //    ýnputActions.Player.Move.performed -= OnMovePerformed;
    //    ýnputActions.Player.Move.canceled -= OnMoveCanceled;
    //}

    //void OnMovePerformed(InputAction.CallbackContext context)
    //{
    //    moveInput = context.ReadValue<Vector2>();
    //}

    //void OnMoveCanceled(InputAction.CallbackContext context)
    //{
    //    moveInput = Vector2.zero;
    //}

    //private void Update()
    //{
    //    Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
    //    transform.Translate(move * Time.deltaTime * 10f);
    //}

    //--------------------------FPS CONTROL-----------------

    CharacterController controller;
    private InputSystem_Actions inputActions;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float runSpeed = 15f;
    bool isSprinting = false;


    [Header("Rotation Settings")]
    [SerializeField] float rotationSpeedX = 5f;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float pitchLimit = 80f;
    [SerializeField] float rotationSpeedY = 15f;

    [Header("Gravity Settings")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float grounOffset = -0.1f;
    Vector3 velocity;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 6f;
    bool jumpPressed = false;


    private Vector2 moveInput;
    private Vector2 lookInput;

    float currentPitch;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;

        inputActions.Player.Sprint.performed += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled += ctx => isSprinting = false;

    }
    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled -= ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed -= ctx => jumpPressed = false;
    }

    private void Update()
    {

        HandleMovement();

        HandleRotation();
        HandleCameraPitch();
        AppleGravity();
    }

    private void HandleCameraPitch()
    {
        //-------Asagý Yukarý Bakýþ--------
        float pitch = -lookInput.y * rotationSpeedY * Time.deltaTime;
        currentPitch += pitch; //mouseden gelen degeri ekledik.
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);
        cameraTransform.localRotation = Quaternion.Euler(currentPitch, 0, 0);
    }

    private void HandleRotation()
    {
        //Saða sola dönüþ ------yaw:Uçus dilinde saða sola dönmek demek.
        float yaw = lookInput.x * rotationSpeedX * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, yaw, 0);
        transform.rotation *= deltaRotation;
    }

    private void HandleMovement()
    {
        // Hareket için gerekli kodlar
        float currentSpeed = isSprinting ? runSpeed : moveSpeed;
        Vector3 forward = transform.forward;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move((move * currentSpeed + velocity) * Time.deltaTime);
    }

    private void AppleGravity()
    {
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = grounOffset;
            }
            if (jumpPressed)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                jumpPressed = false;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
        }
        if (controller.isGrounded&&velocity.y<0)
        {
            velocity.y = grounOffset;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }
}


