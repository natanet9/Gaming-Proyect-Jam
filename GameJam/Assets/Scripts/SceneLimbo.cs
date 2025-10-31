using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLimbo : MonoBehaviour
{
    [SerializeField] PlayerController2 player;
    [SerializeField] MeshRenderer camioneta;
    [SerializeField] Animator animCam;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasTriggered) return;

        if (name == "Stoping")
        {
            player.move = false;
            camioneta.enabled = true;
            hasTriggered = true;

            animCam.Play("New Animation");

            
            GetComponent<Collider>().enabled = false;
        }
        else if (name == "camioneta")
        {
            SceneManager.LoadScene("Limbo");
        }
    }
}