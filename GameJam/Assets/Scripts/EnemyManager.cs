using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("TODOS los enemigos del laberinto (base + extras)")]
    [SerializeField] private GameObject[] enemigos; // Arrastra TODOS aquí

    void Start()
    {
        // Leer cuántas malas decisiones lleva el jugador
        int malasDecisiones = PlayerPrefs.GetInt("MalasDecisiones", 0);
        int totalActivos = 1 + malasDecisiones; // 1 base + extras por error

        Debug.Log($"Activando {totalActivos} enemigos (malas decisiones: {malasDecisiones})");

        // ACTIVAR solo los necesarios
        for (int i = 0; i < enemigos.Length; i++)
        {
            if (enemigos[i] != null)
            {
                bool debeEstarActivo = i < totalActivos;
                enemigos[i].SetActive(debeEstarActivo);
            }
        }
    }
}