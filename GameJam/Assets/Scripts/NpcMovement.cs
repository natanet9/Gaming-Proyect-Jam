using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Configuración general")]
    [SerializeField] private float wanderRadius = 15f;       // Radio de exploración local
    [SerializeField] private float globalRange = 150f;       // Tamaño aproximado del mapa
    [SerializeField] private float speed = 3.5f;             // Velocidad del NPC
    [SerializeField] private float stoppingDistance = 0.2f;  // Distancia para considerar "llegado"
    [SerializeField] private float waitTime = 1.5f;          // Pausa entre movimientos
    [SerializeField] private bool useGlobalWander = true;    // Si es true → explora todo el mapa

    private Vector3 currentTarget;
    private bool isWaiting = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = false;
        agent.updateRotation = true;
    }

    void Start()
    {
        PickNewDestination();
    }

    void Update()
    {
        // Si está esperando, no hace nada
        if (isWaiting) return;

        // Verifica si ha llegado al destino actual
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAndPickNewDestination());
        }
    }

    IEnumerator WaitAndPickNewDestination()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        PickNewDestination();
        isWaiting = false;
    }

    void PickNewDestination()
    {
        Vector3 randomPoint = useGlobalWander ?
                              GetRandomPointOnNavMesh(globalRange) :
                              GetRandomNavMeshPoint(transform.position, wanderRadius);

        if (randomPoint != Vector3.zero)
        {
            currentTarget = randomPoint;
            agent.SetDestination(currentTarget);
        }
        else
        {
            // Si no encuentra punto válido, reintenta
            Debug.LogWarning("No se encontró punto válido. Reintentando...");
            Invoke(nameof(PickNewDestination), 0.5f);
        }
    }

    // 🔹 Puntos aleatorios cerca del NPC
    Vector3 GetRandomNavMeshPoint(Vector3 center, float maxDistance)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * maxDistance;
            randomDir += center;

            if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                if (Vector3.Distance(transform.position, hit.position) > 2f)
                {
                    return hit.position;
                }
            }
        }
        return Vector3.zero;
    }

    // 🔹 Puntos aleatorios en todo el mapa (modo global)
    Vector3 GetRandomPointOnNavMesh(float mapRange)
    {
        for (int i = 0; i < 40; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-mapRange, mapRange),
                0,
                Random.Range(-mapRange, mapRange)
            );

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }

    // 🔹 Gizmo para visualizar el radio
    void OnDrawGizmosSelected()
    {
        Gizmos.color = useGlobalWander ? Color.cyan : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
