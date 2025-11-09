using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nombre de la escena del juego principal (ajústalo según tu proyecto)
    [Header("Configuración")]
    public string sceneToLoad = "Nivel1"; 

    // Método llamado por el botón "Iniciar"
    public void IniciarJuego()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Método llamado por el botón "Salir"
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();


    }
}
