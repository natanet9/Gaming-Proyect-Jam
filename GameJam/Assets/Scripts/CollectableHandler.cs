using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class CollectableHandler : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] private PlayerInput playerInput;

    [Header("Puertas en orden (Agregar en el inspector)")]
    [SerializeField] private Animator[] puertas;

    public int score = 0;


    private void Start()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (scoreText != null)
            scoreText.text = "Desecha todas las botellas: 0/10";
    }

   

    public void IntentarAbrirPuerta()
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
