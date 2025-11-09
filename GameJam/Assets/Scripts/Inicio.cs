using System.Collections;
using UnityEngine;

public class Inicio : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clip;

    [Header("Animaciones")]
    public Animator animador;
    public string animacion1;
    public string animacion2;
    [SerializeField]PlayerController2 player;

    void Start()
    {
        
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(EjecutarAnimacionesDespuesDelAudio());
            player.move = false;
        }
    }

    IEnumerator EjecutarAnimacionesDespuesDelAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        player.move = true; 
        if (animador != null)
        {
            // Usa triggers o bools si tus animaciones están en un Animator Controller
            animador.SetTrigger(animacion1);
            animador.SetTrigger(animacion2);

        }
        
    }
}
