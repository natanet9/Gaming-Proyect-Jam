using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RelatoController : MonoBehaviour
{
    [Header("Audios del Relato")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioInicio;
    [SerializeField] private AudioClip audioOpcion1;
    [SerializeField] private AudioClip audioOpcion2;

    [Header("Botones")]
    [SerializeField] private GameObject panelBotones;
    [SerializeField] private Button boton1;
    [SerializeField] private Button boton2;

    [Header("ENEMIGOS (desactivados) - Activar según opción")]
    [SerializeField] private Enemy[] enemigosExtra; // Array de enemigos DESACTIVADOS

    [Header("Siguiente Escena")]
    [SerializeField] private string escenaSiguiente = "Laberinto1"; // Cambia por tu escena

    private int enemigosMalasDecisiones = 0; // Contador acumulado

    void Start()
    {
        // Ocultar botones
        panelBotones.SetActive(false);

        // Conectar botones
        boton1.onClick.AddListener(() => Elegir(1));
        boton2.onClick.AddListener(() => Elegir(2));

        // CARGAR CONTADOR DE MALAS DECISIONES (acumula entre escenas)
        enemigosMalasDecisiones = PlayerPrefs.GetInt("MalasDecisiones", 0);

        // Empezar relato
        StartCoroutine(IniciarRelato());
    }

    IEnumerator IniciarRelato()
    {
        yield return new WaitForSeconds(0.5f);

        // 1. Audio inicial
        yield return Reproducir(audioInicio);

        // 2. Mostrar botones
        panelBotones.SetActive(true);
    }

    void Elegir(int opcion)
    {
        // Ocultar botones
        panelBotones.SetActive(false);

        AudioClip clip = opcion == 1 ? audioOpcion1 : audioOpcion2;

        StartCoroutine(ReproducirYContinuar(clip, opcion));
    }

    IEnumerator ReproducirYContinuar(AudioClip clip, int opcion)
    {
        // Reproducir audio elegido
        yield return Reproducir(clip);

        // *** LOGÍCA DE ENEMIGOS ***
        if (opcion == 1) // MALA decisión → MÁS enemigos
        {
            enemigosMalasDecisiones++;
            PlayerPrefs.SetInt("MalasDecisiones", enemigosMalasDecisiones);
            Debug.Log($"❌ Mala decisión! Enemigos extras: {enemigosMalasDecisiones}");
        }
        else // BUENA decisión → Resetear contador
        {
            enemigosMalasDecisiones = 0;
            PlayerPrefs.SetInt("MalasDecisiones", 0);
            Debug.Log("✅ Buena decisión! Enemigos mínimos.");
        }

        // ACTIVAR ENEMIGOS según contador
        ActivarEnemigos();

        // *** CARGAR SIGUIENTE ESCENA ***
        yield return new WaitForSeconds(1f); // Pausa dramática
        SceneManager.LoadScene(escenaSiguiente);
    }

    void ActivarEnemigos()
    {
        int totalEnemigosBase = 1; // Mínimo siempre
        int totalEnemigos = totalEnemigosBase + enemigosMalasDecisiones;

        Debug.Log($"🎮 Activando {totalEnemigos} enemigos (base + {enemigosMalasDecisiones} malas decisiones)");

        for (int i = 0; i < enemigosExtra.Length; i++)
        {
            if (enemigosExtra[i] != null)
            {
                // Activar solo los necesarios
                bool activar = i < totalEnemigos;
                enemigosExtra[i].gameObject.SetActive(activar);
            }
        }
    }

    IEnumerator Reproducir(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }
}