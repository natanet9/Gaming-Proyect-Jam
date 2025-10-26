using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLimbo : MonoBehaviour
{
    [SerializeField] PlayerController2 player;
    [SerializeField] GameObject camioneta;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasTriggered) return;

        if (name == "Stoping")
        {
            player.move = false;
            camioneta.SetActive(true);
            hasTriggered = true;

            
            GetComponent<Collider>().enabled = false;
        }
        else if (name == "camioneta")
        {
            SceneManager.LoadScene("Limbo");
        }
    }
}