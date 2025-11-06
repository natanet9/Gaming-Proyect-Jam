using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReflexion : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Camera cam;
    [SerializeField] private float mouse_horizontal = 1.0f;
    [SerializeField] private float mouse_vertical = 1.0f;
    [SerializeField] private float maxRotation = 60.0f;

    private float hMouse = 0f;
    private float vMouse = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        Vector2 input = playerInput.actions["MoveCamera"].ReadValue<Vector2>();

        hMouse += input.x * mouse_horizontal;
        vMouse -= input.y * mouse_vertical;

       
        hMouse = Mathf.Clamp(hMouse, -maxRotation, maxRotation);
        vMouse = Mathf.Clamp(vMouse, -maxRotation, maxRotation);

     
        cam.transform.localEulerAngles = new Vector3(vMouse, hMouse, 0);
    }
}
