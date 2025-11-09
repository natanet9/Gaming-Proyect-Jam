using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneLimbo : MonoBehaviour
{
    [SerializeField] PlayerController2 player;
    [SerializeField] GameObject camioneta;
    [SerializeField] Animator animCam;
    [SerializeField] VideoPlayer videoPlayer; 
 

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasTriggered) return;

        if (name == "Stoping")
        {
            player.move = false;
            camioneta.SetActive(true);
            hasTriggered = true;

            animCam.Play("New Animation");

            GetComponent<Collider>().enabled = false;
        }
        else if (name == "camioneta")
        {
           
            StartCoroutine(ReproducirVideoYCambiarEscena());
        }
    }

   private System.Collections.IEnumerator ReproducirVideoYCambiarEscena()
{
    if (videoPlayer != null)
    {
        videoPlayer.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f); // 🔹 pequeño retraso
        videoPlayer.Play();

        Debug.Log("🎥 Video iniciado: " + videoPlayer.clip.name);

        // Espera hasta que empiece realmente
        while (!videoPlayer.isPlaying)
            yield return null;

        Debug.Log("🎬 Video reproduciéndose...");

        // Espera a que termine
        while (videoPlayer.isPlaying)
            yield return null;
    }

    Debug.Log("✅ Video terminado, cambiando escena...");

    SceneManager.LoadScene("Limbo");
}

}
