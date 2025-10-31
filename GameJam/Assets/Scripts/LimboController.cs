using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LimboController : MonoBehaviour
{
    [Header("Referencias a objetos (asigna en el Inspector)")]
    public GameObject puertaNivel1;
    public GameObject puertaNivel2;
    public GameObject puertaNivel3;
    public GameObject puertaFinal;

    public Text textoDialogo;               // Opcional: UI Text para mostrar diálogos
    public Button botonEntrarNivel;         // Botón opcional para entrar al nivel seleccionado

    [Header("Nombres de las escenas de niveles")]
    public string escenaNivel1 = "Nivel1";
    public string escenaNivel2 = "Nivel2";
    public string escenaNivel3 = "Nivel3";
    public string escenaFinal = "Final";

    [Header("Diálogos por visita")]
    public List<string> dialogos = new List<string>
    {
        "¡Bienvenido al Limbo por primera vez! La puerta al Nivel 1 se ha abierto.",   // Visita 1
        "Has regresado del Nivel 1. La puerta al Nivel 2 ahora está disponible.",      // Visita 2
        "¡Nivel 2 completado! La puerta al Nivel 3 se abre.",                         // Visita 3
        "Última prueba superada. La puerta al Final te espera."                       // Visita 4
    };

    private int nivelActualAPoner;  // Para saber qué puerta activar

    void Start()
    {
        // Incrementamos la visita al cargar la escena
        ProgresoLimbo.IncrementarVisita();
        int visita = ProgresoLimbo.Visitas;

        // Seguridad: no debería haber más de 4 visitas
        if (visita > 4) visita = 4;

        // Mostrar diálogo correspondiente
        MostrarDialogo(visita - 1);

        // Activar la puerta según la visita
        ActivarPuertaSegunVisita(visita);

        // Configurar botón si existe
        if (botonEntrarNivel != null)
        {
            botonEntrarNivel.onClick.RemoveAllListeners();
            botonEntrarNivel.onClick.AddListener(IrAlSiguienteNivel);
            botonEntrarNivel.gameObject.SetActive(true);
        }
    }

    void MostrarDialogo(int indice)
    {
        if (textoDialogo != null && indice >= 0 && indice < dialogos.Count)
        {
            textoDialogo.text = dialogos[indice];
        }
    }

    void ActivarPuertaSegunVisita(int visita)
    {
        // Desactivar todas las puertas primero
        puertaNivel1.SetActive(false);
        puertaNivel2.SetActive(false);
        puertaNivel3.SetActive(false);
        puertaFinal.SetActive(false);

        switch (visita)
        {
            case 1:
                puertaNivel1.SetActive(true);
                nivelActualAPoner = 1;
                break;
            case 2:
                if (ProgresoLimbo.EstaNivelCompletado(1))
                {
                    puertaNivel2.SetActive(true);
                    nivelActualAPoner = 2;
                }
                else
                {
                    // Seguridad: si volvió sin completar, lo mandamos de nuevo al 1
                    puertaNivel1.SetActive(true);
                    nivelActualAPoner = 1;
                }
                break;
            case 3:
                if (ProgresoLimbo.EstaNivelCompletado(2))
                {
                    puertaNivel3.SetActive(true);
                    nivelActualAPoner = 3;
                }
                else
                {
                    puertaNivel2.SetActive(true);
                    nivelActualAPoner = 2;
                }
                break;
            case 4:
                if (ProgresoLimbo.EstaNivelCompletado(3))
                {
                    puertaFinal.SetActive(true);
                    nivelActualAPoner = 4;
                }
                else
                {
                    puertaNivel3.SetActive(true);
                    nivelActualAPoner = 3;
                }
                break;
        }
    }

    // Llamado por el botón o por colisión con la puerta
    public void IrAlSiguienteNivel()
    {
        string escenaDestino = nivelActualAPoner switch
        {
            1 => escenaNivel1,
            2 => escenaNivel2,
            3 => escenaNivel3,
            4 => escenaFinal,
            _ => escenaNivel1
        };

        SceneManager.LoadScene(escenaDestino);
    }


}