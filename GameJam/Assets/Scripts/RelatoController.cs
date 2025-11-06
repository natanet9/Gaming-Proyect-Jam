using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RelatoController : MonoBehaviour
{
    [Header("Audios del Relato")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioInicio;
    [SerializeField] private AudioClip audioOpcion1;
    [SerializeField] private AudioClip audioOpcion2;

    [Header("Botones (aparecen después del audio inicial)")]
    [SerializeField] private GameObject panelBotones;
    [SerializeField] private Button boton1;
    [SerializeField] private Button boton2;

    void Start()
    {
        // Ocultar botones al inicio
        panelBotones.SetActive(false);

        // Conectar botones
        boton1.onClick.AddListener(() => Elegir(1));
        boton2.onClick.AddListener(() => Elegir(2));

        // Empezar el relato automáticamente
        StartCoroutine(IniciarRelato());
    }

    IEnumerator IniciarRelato()
    {
        yield return new WaitForSeconds(0.5f); // Pequeña pausa al cargar

        // 1. Reproducir audio inicial
        yield return Reproducir(audioInicio);

        // 2. Mostrar botones
        panelBotones.SetActive(true);
    }

    void Elegir(int opcion)
    {
        // Ocultar botones
        panelBotones.SetActive(false);

        // Reproducir audio elegido
        AudioClip clip = opcion == 1 ? audioOpcion1 : audioOpcion2;
        StartCoroutine(Reproducir(clip));
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