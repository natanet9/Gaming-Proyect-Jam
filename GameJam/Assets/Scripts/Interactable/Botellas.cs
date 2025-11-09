using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botellas : Interactable
{
    CollectableHandler collectable;

    // Start is called before the first frame update
    void Start()
    {
        collectable = FindFirstObjectByType<CollectableHandler>();
    }
    public override void Interact()
    {
        base.Interact();
        Destroy(gameObject);
        collectable.score += 1;
        collectable.scoreText.text = $"Desecha todas las botellas: {collectable.score}/10";
        collectable.IntentarAbrirPuerta();
        if (collectable.score == 10)
        {
            collectable.scoreText.text = "Busca ala salida";
        }
    }   
}
