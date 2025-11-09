using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public static bool PlayerIsSafe = false;
    [SerializeField] GameObject panelZonSafe;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIsSafe = true;
            panelZonSafe.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIsSafe = false;
            panelZonSafe.SetActive(false);
        }
            
    }
}
