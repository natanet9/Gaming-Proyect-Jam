using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Transform player;      // Asigna aquí el jugador en el inspector
    [SerializeField] float rotationSpeed = 5f;  // Velocidad de rotación

    void Update()
    {
        if (player == null) return;

        // Calcula la dirección hacia el jugador
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // Evita inclinarse hacia arriba o abajo

        // Si hay dirección válida, rota suavemente
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
