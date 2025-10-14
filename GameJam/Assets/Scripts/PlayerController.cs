using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedMove = 3.0f;
    PlayerInput actionMap;

    // Start is called before the first frame update
    void Start()
    {
        actionMap = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    public void MovePlayer()
    {
            Vector2 inputs = actionMap.actions["Move"].ReadValue<Vector2>();
            transform.Translate(new Vector3(inputs.x, 0, inputs.y) * speedMove * Time.deltaTime);
    }
}
