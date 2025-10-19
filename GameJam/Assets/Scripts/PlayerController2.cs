using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController controller;
    Vector2 inputs;
    [SerializeField]Animator anim;
    [SerializeField] float SpeedMove = 3f;
    float hMouse, vMouse;
    [SerializeField] float mouse_horizontal = 1.0f;
    [SerializeField] float mouse_vertical = 1.0f;
    float maxrotationLookUp = -60.0f;
     float maxrotationLookDown = 60.0f;
    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
       MoveCamera();
    }
    void MovePlayer()
    {
        inputs = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(inputs.x, 0, inputs.y);
        controller.Move(transform.TransformDirection(move) * SpeedMove * Time.deltaTime);
        bool ismoving = move.magnitude != 0;
        anim.SetBool("Walk", ismoving);

    }
    void MoveCamera()
    {
        Vector2 input = playerInput.actions["MoveCamera"].ReadValue<Vector2>();
        hMouse = mouse_horizontal * input.x;
        vMouse += mouse_vertical * input.y;
        vMouse = Math.Clamp(vMouse, maxrotationLookUp, maxrotationLookDown);
        cam.transform.localEulerAngles = new Vector3(-vMouse, 0, 0);
        transform.Rotate(0, hMouse, 0);
    }
 
}
