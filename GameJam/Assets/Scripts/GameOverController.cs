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

        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void IrAlMenu()
    {
        Time.timeScale = 1f;
        ProgresoLimbo.Reset(); // opcional: borra progreso
        SceneManager.LoadScene("MainMenu");
    }
}
