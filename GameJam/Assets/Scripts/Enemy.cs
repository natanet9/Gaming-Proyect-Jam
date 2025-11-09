using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Video;

public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float persecutionRadius = 8f;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float normalSpeed = 3.5f;
    [SerializeField] float chaseSpeed = 6f;
    [SerializeField] Transform player;
    [SerializeField] LayerMask isPlayer;

    [Header("Audio CONTINUO")]
    [SerializeField] AudioSource sourceLoop;   // Pasos (loop)
    [SerializeField] AudioSource sourceOneShot; // Gritos
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip run;
    [SerializeField] AudioClip scream;

    [Header("SCREAMER DE MUERTE")]
    [SerializeField] RawImage screamerImage;
    [SerializeField] VideoPlayer screamerVideo;
    [SerializeField] GameObject gameOverPanel;

    Animator anim;
    NavMeshAgent agent;

    int currentPoint = 0;
    bool waiting = false;
    bool yaMato = false;
    bool isChasing = false;

   void Start()
{
    agent = GetComponent<NavMeshAgent>();
    anim = GetComponent<Animator>();

    if (agent == null)
    {
        Debug.LogError("Falta el componente NavMeshAgent en " + gameObject.name);
        enabled = false;
        return;
    }

    agent.speed = normalSpeed;

    if (sourceLoop)
    {
        sourceLoop.loop = true;
        sourceLoop.clip = walk;
        sourceLoop.Play();
    }


    if (patrolPoints.Length > 0)
    {
        currentPoint = Random.Range(0, patrolPoints.Length);
        GoToNextPoint();
    }
}

    void Update()
    {
        if (yaMato) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador está en zona segura → patrulla
        if (SafeZone.PlayerIsSafe)
        {
            if (isChasing)
            {
                DetenerPersecucion();
            }

            Patrol();
            return;
        }

        // Si el jugador está dentro del radio de ataque
        if (distToPlayer <= attackRadius)
        {
            Attack();
        }
        // Si está dentro del radio de persecución
        else if (distToPlayer <= persecutionRadius)
        {
            Chase();  // persecución en tiempo real
        }
        else
        {
            Patrol();
        }
    }

    // Patrullaje básico
    void Patrol()
    {
        if (isChasing)
        {
            DetenerPersecucion();
        }

        if (waiting || agent.pathPending) return;

        if (agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    // Persecución en tiempo real
    void Chase()
    {
        if (!player) return;

        if (!isChasing)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
            CambiarLoop(run);
            anim.SetBool("Run", true);
        }

        // Sigue al jugador continuamente (no cada cierto tiempo)
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void DetenerPersecucion()
    {
        isChasing = false;
        agent.speed = normalSpeed;
        CambiarLoop(walk);
        anim.SetBool("Run", false);
    }

    // Ataque con screamer
    void Attack()
    {
        if (yaMato || SafeZone.PlayerIsSafe) return;

        yaMato = true;
        agent.isStopped = true;
        anim.SetTrigger("Attack");

        // Mira hacia el jugador antes de atacar
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);

        StartCoroutine(ScreamerMuerte());
    }

    IEnumerator ScreamerMuerte()
    {
        if (sourceLoop) sourceLoop.Stop();
        if (sourceOneShot && scream) sourceOneShot.PlayOneShot(scream);

        yield return new WaitForSeconds(0.3f);

        screamerImage.gameObject.SetActive(true);
        screamerVideo.Play();

        yield return new WaitForSeconds(3f);

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

   IEnumerator WaitAndMove()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);

        if (patrolPoints.Length > 1)
        {
            int nuevoPunto;
            do
            {
                nuevoPunto = Random.Range(0, patrolPoints.Length);
            }
            while (nuevoPunto == currentPoint); 

            currentPoint = nuevoPunto;
        }

        GoToNextPoint();
        waiting = false;
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    void CambiarLoop(AudioClip nuevoClip)
    {
        if (sourceLoop == null || nuevoClip == null) return;

        if (sourceLoop.clip != nuevoClip)
        {
            sourceLoop.clip = nuevoClip;
            sourceLoop.Play();
        }
        else if (!sourceLoop.isPlaying)
        {
            sourceLoop.Play();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, persecutionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
