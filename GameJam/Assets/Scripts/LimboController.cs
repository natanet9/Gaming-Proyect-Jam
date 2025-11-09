using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
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

    [Header("Video (solo para escena final)")]
    public VideoPlayer videoPlayer; // 🎥 asignar en el inspector
    public string escenaMenu = "Menu"; // escena del menú principal

    [Header("Nombres de escenas")]
    public string escenaNivel1 = "Nivel1";
    public string escenaNivel2 = "Nivel2";
    public string escenaFinal = "Final";

    private int nivelActual;
    private bool saltarAudio = false;
    [SerializeField] AudioSource musica;

    void Start()
    {
        ProgresoLimbo.IncrementarVisita();
        int visita = ProgresoLimbo.Visitas;

        if (visita > 3) visita = 3;

        StartCoroutine(ReproducirDialogoYActivarPuerta(visita));
    }

    void Update()
    {
        // Si se presiona F, marcar que el audio debe saltarse
        if (Input.GetKeyDown(KeyCode.F))
        {
            saltarAudio = true;
            if (audioSource && audioSource.isPlaying)
                audioSource.Stop();
        }
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

        // Reproducir audio (permitiendo saltarlo)
        if (audioSource && clip)
        {
            saltarAudio = false;
            audioSource.clip = clip;
            audioSource.Play();

            // Esperar hasta que el audio termine o se salte
            yield return new WaitWhile(() => audioSource.isPlaying && !saltarAudio);
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

        // Si es la escena final, reproducir video antes de ir al menú
        if (nivelActual == 3 && videoPlayer != null)
        {
            StartCoroutine(ReproducirVideoYCargarMenu());
        }
        else
        {
            SceneManager.LoadScene(escena);
        }
    }

    IEnumerator ReproducirVideoYCargarMenu()
    {
        videoPlayer.gameObject.SetActive(true);
        musica.Stop();
        videoPlayer.Play();

        // Esperar hasta que empiece realmente
        while (!videoPlayer.isPlaying)
            yield return null;

        // Esperar hasta que termine el video
        while (videoPlayer.isPlaying)
            yield return null;

        SceneManager.LoadScene(escenaMenu);
    }
}
