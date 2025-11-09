using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    CharacterController controller;
    Vector2 inputs;
    [SerializeField] Animator anim;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 10f;
    float currentSpeed;

    float hMouse, vMouse;
    [SerializeField] float mouse_horizontal = 1.0f;
    [SerializeField] float mouse_vertical = 1.0f;
    float maxrotationLookUp = -60.0f;
    float maxrotationLookDown = 60.0f;
    [SerializeField] Camera cam;
    [SerializeField] float gravity = -9.81f;
    public bool move = true;
    Vector3 velocity;
    bool isgrounded;

    [SerializeField] bool Scenerunning = true;
    [HideInInspector] public StamineController stamina;

    [SerializeField] float distSuelo = 1.5f;
    [SerializeField] LayerMask groundMask;
     public bool evitarCaidas = true;  

    private bool isRunning = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;

        if (Scenerunning)
        {
            stamina = GetComponent<StamineController>();
        }
    }

    void Update()
    {
        if (move)
        {
            MovePlayer();
        if (Scenerunning)
        {
            HandleSprint();
        }
        }
      
        MoveCamera();
    }

    void HandleSprint()
    {
        // Leer si se mantiene presionado "Run" (Shift)
        bool runInput = playerInput.actions["Run"].IsPressed();
        bool isMoving = inputs.magnitude > 0.1f;

        // Condiciones para correr
        isRunning = runInput && isMoving && stamina.CanRun();

        // Ajustar velocidad
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Animaciones
        anim.SetBool("Walk", isMoving && !isRunning);
        anim.SetBool("Run", isRunning);

        // Consumir stamina
        if (isRunning)
        {
            stamina.Running(); // Drena stamina
        }
    }

  void MovePlayer()
    {
         inputs = playerInput.actions["Move"].ReadValue<Vector2>(); 
        Vector3 move = new Vector3(inputs.x, 0, inputs.y);
        move = transform.TransformDirection(move).normalized;

        Vector3 basePos = transform.position + Vector3.down * (controller.height / 2f - controller.skinWidth);
        bool allowMove = true;

        if (evitarCaidas && move.magnitude > 0.1f) 
        {
            float offset = 0.3f;
            Vector3 rayOrigin = basePos + move * offset;
            bool groundAhead = Physics.Raycast(rayOrigin, Vector3.down, distSuelo, groundMask);

            if (!groundAhead && controller.isGrounded)
            {
                allowMove = false;
            }

            Debug.DrawRay(rayOrigin, Vector3.down * distSuelo, groundAhead ? Color.green : Color.red);
        }

        // Movimiento
        if (allowMove)
            controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravedad
        isgrounded = controller.isGrounded;
        if (isgrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }



    void MoveCamera()
    {
        Vector2 input = playerInput.actions["MoveCamera"].ReadValue<Vector2>();
        hMouse += mouse_horizontal * input.x;
        vMouse -= mouse_vertical * input.y;
        vMouse = Mathf.Clamp(vMouse, maxrotationLookUp, maxrotationLookDown);

        cam.transform.localEulerAngles = new Vector3(vMouse, 0, 0);
        transform.Rotate(Vector3.up * hMouse);
        hMouse = 0; // Reset para evitar acumulación
    }
}