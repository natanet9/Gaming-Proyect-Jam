using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class InteractRaycast : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask layermask;

    [SerializeField] GameObject panelText;
    PlayerInput input;

    [SerializeField] Transform camara;
    [SerializeField] AudioClip Interact;
    AudioSource audiosource;
    void Start()
    {
        input = GetComponent<PlayerInput>();
        panelText.SetActive(false);
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(camara.position, camara.forward * rayDistance, Color.red);
        RaycastHit hit;
        if(Physics.Raycast(camara.position, camara.forward,out hit, rayDistance,layermask))
        {
            panelText.SetActive(true);
            if (input.actions["Interact"].triggered)
            {
                hit.transform.GetComponent<Interactable>().Interact();
                audiosource.clip = Interact;
                audiosource.Play();
            }
        }
        else
        {
            panelText.SetActive(false);
        }
    }
}
