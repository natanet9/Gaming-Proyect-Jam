using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CollectableHandler : MonoBehaviour
{
    [SerializeField] private string collectableTag = "ObjCerveza";
    [SerializeField] private Text scoreText;
    [SerializeField] private PlayerInput playerInput;

    [Header("Puertas en orden (Agregar en el inspector)")]
    [SerializeField] private Animator[] puertas;

    private int score = 0;
    private Collider currentCollectable;

    private void Start()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (scoreText != null)
            scoreText.text = "Contador: 0";
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar coleccionables
        if (other.CompareTag(collectableTag))
        {
            currentCollectable = other;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentCollectable)
        {
            currentCollectable = null;
        }
    }

    private void Update()
    {
        if (currentCollectable != null && playerInput.actions["ObjEncontrar"].WasPressedThisFrame())
        {
            Destroy(currentCollectable.gameObject);
            currentCollectable = null;

            score++;
            IntentarAbrirPuerta();
            if (scoreText != null)
                scoreText.text = "Contador: " + score;
        }
    }

    private void IntentarAbrirPuerta()
    {
        int indexPuerta = score - 1;

        // Validar que el score coincida con una puerta existente
        if (indexPuerta >= 0 && indexPuerta < puertas.Length)
        {
            Animator puertaAnim = puertas[indexPuerta];

            if (puertaAnim != null)
            {
                puertaAnim.SetBool("abrir",true);
            }
        }
    }
}
