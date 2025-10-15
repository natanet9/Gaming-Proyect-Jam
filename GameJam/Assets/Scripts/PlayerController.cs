using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController controller;
    Vector2 inputs;
    [SerializeField] float SpeedMove = 3f;
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
    }
    void MovePlayer()
    {
        inputs = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(inputs.x, 0, inputs.y);
        controller.Move(transform.TransformDirection(move)*SpeedMove*Time.deltaTime);
    }
    
}
