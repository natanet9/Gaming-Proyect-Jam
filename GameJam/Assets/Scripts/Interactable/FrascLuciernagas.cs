using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrascLuciernagas : Interactable
{
    Laberinto2 lab;

    void Start()
    {
        lab = FindFirstObjectByType<Laberinto2>();
    }
    public override void Interact()
    {
        base.Interact();
        lab.interact();
        Destroy(gameObject);
    }
}
