using UnityEngine;

public class NextLevelCollider : MonoBehaviour
{
    [SerializeField] private LimboController limbo;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            limbo.IrAlSiguienteNivel();
        }
    }
}