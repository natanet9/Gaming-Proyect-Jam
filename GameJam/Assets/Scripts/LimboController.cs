using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LimboController : MonoBehaviour
{
    [Header("Puertas")]
    public GameObject puertaNivel1;
    public GameObject puertaNivel2;
    public GameObject puertaFinal;

    [Header("Audios (diálogos)")]
    public AudioSource audioSource;
    public AudioClip dialogo1; // "Bienvenido al Limbo..."
    public AudioClip dialogo2; // "Has regresado del Nivel 1..."
    public AudioClip dialogo3; // "El Final te espera..."

    [Header("Nombres de escenas")]
    public string escenaNivel1 = "Nivel1";
    public string escenaNivel2 = "Nivel2";
    public string escenaFinal = "Final";

    private int nivelActual;

    void Start()
    {
        ProgresoLimbo.IncrementarVisita();
        int visita = ProgresoLimbo.Visitas;

        // Seguridad
        if (visita > 3) visita = 3;

        // Reproducir diálogo y activar puerta
        StartCoroutine(ReproducirDialogoYActivarPuerta(visita));
    }

    IEnumerator ReproducirDialogoYActivarPuerta(int visita)
    {
        // Desactivar todas las puertas
        puertaNivel1.SetActive(false);
        puertaNivel2.SetActive(false);
        puertaFinal.SetActive(false);

        AudioClip clip = visita switch
        {
            1 => dialogo1,
            2 => dialogo2,
            3 => dialogo3,
            _ => dialogo1
        };

        // Reproducir audio
        if (audioSource && clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        // Activar puerta según visita
        switch (visita)
        {
            case 1:
                puertaNivel1.SetActive(true);
                nivelActual = 1;
                break;
            case 2:
                if (ProgresoLimbo.EstaNivelCompletado(1))
                {
                    puertaNivel2.SetActive(true);
                    nivelActual = 2;
                }
                else
                {
                    puertaNivel1.SetActive(true);
                    nivelActual = 1;
                }
                break;
            case 3:
                if (ProgresoLimbo.EstaNivelCompletado(2))
                {
                    puertaFinal.SetActive(true);
                    nivelActual = 3;
                }
                else
                {
                    puertaNivel2.SetActive(true);
                    nivelActual = 2;
                }
                break;
        }
    }

    public void IrAlSiguienteNivel()
    {
        string escena = nivelActual switch
        {
            1 => escenaNivel1,
            2 => escenaNivel2,
            3 => escenaFinal,
            _ => escenaNivel1
        };

        SceneManager.LoadScene(escena);
    }
}