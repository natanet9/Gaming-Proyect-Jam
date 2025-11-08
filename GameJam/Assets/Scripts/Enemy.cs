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
    [SerializeField] AudioSource sourceLoop;   // Sonidos de pasos (LOOP)
    [SerializeField] AudioSource sourceOneShot; // Gritos (una vez)
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip run;
    [SerializeField] AudioClip scream;

    Animator anim;
    NavMeshAgent agent;
    int currentPoint = 0;
    bool waiting = false;
    [Header("SCREAMER DE MUERTE")]
    [SerializeField] RawImage screamerImage;  
    [SerializeField] VideoPlayer screamerVideo;  
    [SerializeField] GameObject gameOverPanel;   
    private bool yaMato = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = normalSpeed;

        // Asegurar que el loop esté activo
        if (sourceLoop)
        {
            sourceLoop.loop = true;
            sourceLoop.clip = walk;
            sourceLoop.Play();
        }

        if (patrolPoints.Length > 0)
            GoToNextPoint();
    }

    void Update()
    {
        Patroll();
        Chase();
        Attack();
    }

    void Patroll()
    {
        if (waiting || agent.pathPending) return;

        if (agent.remainingDistance < 0.5f)
        {
            CambiarLoop(walk);
            anim.SetBool("Run", false);
            StartCoroutine(WaitAndMove());
        }
    }

    void Chase()
    {
        bool veJugador = Physics.CheckSphere(transform.position, persecutionRadius, isPlayer);

        if (veJugador)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            CambiarLoop(run);
            anim.SetBool("Run", true);
        }
        else
        {
            agent.speed = normalSpeed;
            CambiarLoop(walk);
            anim.SetBool("Run", false);
        }
    }

   void Attack()
{
    if (yaMato) return;

    if (Physics.CheckSphere(transform.position, attackRadius, isPlayer))
    {
        if (!agent.isStopped)
        {
            agent.isStopped = true;
            anim.SetTrigger("Attack");
            
            // SCREAMER DE MUERTE
            StartCoroutine(ScreamerMuerte());
            yaMato = true;
        }
    }
}

IEnumerator ScreamerMuerte()
{
    yaMato = true;


    if (sourceLoop) sourceLoop.Stop();


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
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
        GoToNextPoint();
        waiting = false;
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    // EL TRUCO: Cambia el LOOP sin parar el audio
    void CambiarLoop(AudioClip nuevoClip)
    {
        if (sourceLoop == null || nuevoClip == null) return;

        if (sourceLoop.clip != nuevoClip)
        {
            sourceLoop.clip = nuevoClip;
            if (!sourceLoop.isPlaying)
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