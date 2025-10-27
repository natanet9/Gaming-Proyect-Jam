using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // ← ESTO FALTABA

public class StamineController : MonoBehaviour
{
    public float staminePlayer = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [HideInInspector] public bool hasregenerate = true;

    [SerializeField] private float DrainStamina = 20f;
    [SerializeField] private float RegenerateStamina = 15f;

    [SerializeField] private Image StaminaBar;
    [SerializeField] private CanvasGroup GrupCanvas;

    private PlayerController2 player;
    private PlayerInput playerInput; // ← Ahora lo declaramos aquí

    void Start()
    {
        player = GetComponent<PlayerController2>();
        playerInput = GetComponent<PlayerInput>(); // ← Lo obtenemos directamente
        staminePlayer = maxStamina;
        UpdateStaminaUI();
        GrupCanvas.alpha = 0;
    }

    void Update()
    {
        if (!IsRunning() && staminePlayer < maxStamina)
        {
            staminePlayer += RegenerateStamina * Time.deltaTime;
            staminePlayer = Mathf.Min(staminePlayer, maxStamina);
            UpdateStaminaUI();
            GrupCanvas.alpha = 1;
        }

        if (staminePlayer >= maxStamina)
        {
            GrupCanvas.alpha = 0;
            hasregenerate = true;
        }
    }

    public void Running()
    {
        if (CanRun())
        {
            staminePlayer -= DrainStamina * Time.deltaTime;
            staminePlayer = Mathf.Max(staminePlayer, 0);
            UpdateStaminaUI();
            GrupCanvas.alpha = 1;

            if (staminePlayer <= 0)
            {
                hasregenerate = false;
            }
        }
    }

    public bool CanRun()
    {
        return staminePlayer > 0 && hasregenerate;
    }

    private bool IsRunning()
    {
        if (playerInput == null || playerInput.actions == null) return false;

        var moveAction = playerInput.actions["Move"];
        var runAction = playerInput.actions["Run"];

        bool moving = moveAction != null && moveAction.ReadValue<Vector2>().magnitude > 0.1f;
        bool holdingRun = runAction != null && runAction.IsPressed();

        return moving && holdingRun;
    }

    void UpdateStaminaUI()
    {
        if (StaminaBar != null)
        {
            StaminaBar.fillAmount = staminePlayer / maxStamina;
        }
    }
}