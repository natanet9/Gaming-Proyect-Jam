using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Necesario para usar PlayerInput

public class CollectableHandler : MonoBehaviour
{
    [SerializeField] private string collectableTag = "ObjCerveza"; // Tag del objeto
    [SerializeField] private Text scoreText; // Texto del contador
    [SerializeField] private PlayerInput playerInput; // Referencia al PlayerInput

    private int score = 0;
    private Collider currentCollectable; // Guarda el objeto con el que colisionas

    private void Start()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (scoreText != null)
            scoreText.text = "Contador: 0";
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si colisiona con un objeto con el tag
        if (other.CompareTag(collectableTag))
        {
            currentCollectable = other; // Guardamos la referencia
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si sale del área del objeto, limpiamos la referencia
        if (other == currentCollectable)
        {
            currentCollectable = null;
        }
    }

    private void Update()
    {
        // Si el jugador presiona el botón "ObjEncontrar" y hay un objeto cerca
        if (currentCollectable != null && playerInput.actions["ObjEncontrar"].WasPressedThisFrame())
        {
            // Elimina el objeto
            Destroy(currentCollectable.gameObject);
            currentCollectable = null;

            // Incrementa el contador
            score++;

            // Actualiza el texto
            if (scoreText != null)
            {
                scoreText.text = "Contador: " + score;
            }
        }
    }
}
