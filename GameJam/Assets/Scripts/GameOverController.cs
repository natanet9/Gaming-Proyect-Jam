using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Button botonReiniciar;
    [SerializeField] Button botonMenu;

    void Start()
    {
        botonReiniciar.onClick.AddListener(ReiniciarNivel);
        botonMenu.onClick.AddListener(IrAlMenu);
    }

    void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        // Vuelve al Limbo (mantiene progreso)
        SceneManager.LoadScene("Limbo");
    }

    void IrAlMenu()
    {
        Time.timeScale = 1f;
        ProgresoLimbo.Reset(); // Opcional: borra progreso
        SceneManager.LoadScene("MainMenu"); // Cambia por tu menú
    }
}