using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalNivel : MonoBehaviour
{
    [Header("Configuración del nivel actual")]
    public int numeroNivel = 1; 

    string escenaLimbo = "Limbo";

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Marca este nivel como completado
            ProgresoLimbo.MarcarNivelCompletado(numeroNivel);
            Debug.Log($"Nivel {numeroNivel} completado");

            // Carga la escena del Limbo (o siguiente nivel)
            SceneManager.LoadScene(escenaLimbo);
        }
    }
}
