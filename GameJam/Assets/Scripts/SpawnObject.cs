using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject miPrefab;


    public int cantidad = 20;


    public float anchoX = 100f;      
    public float largoZ = 100f;  
    public float alturaY = 1f;


    public Color colorGizmo ;


    void Start()
    {
        Vector3 centro = transform.position;

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = new Vector3(
                centro.x + Random.Range(-anchoX * 0.5f, anchoX * 0.5f),
                centro.y + alturaY,
                centro.z + Random.Range(-largoZ * 0.5f, largoZ * 0.5f)
            );

            Instantiate(miPrefab, pos, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 centro = transform.position;
        Vector3 tamaño = new Vector3(anchoX, 0.1f, largoZ);

        Gizmos.color = colorGizmo;
        Gizmos.DrawWireCube(centro, tamaño);

    }
}