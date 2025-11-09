using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Laberinto2 : MonoBehaviour
{
    [SerializeField] InteractRaycast raycast;
    [SerializeField] GameObject salida;
    [SerializeField] TextMeshProUGUI textcant;
     SpawnObject Object;
    int recolectObjects;
    void Start()
    {
        Object = GetComponent<SpawnObject>();

        recolectObjects = Object.cantidad;

        textcant.text=$"Recolecta tus esperanzas por la vida: {recolectObjects}";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void interact()
    {
        recolectObjects -= 1;
        textcant.text = $"Recolecta tus esperanzas por la vida: {recolectObjects}";
    }
}
