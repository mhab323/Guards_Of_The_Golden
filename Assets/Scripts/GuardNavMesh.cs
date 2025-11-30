using UnityEngine;
using UnityEngine.AI;

public class GuardNavMesh : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // Drag Player here
    private Health playerHealth;

    private NavMeshAgent agent;
    private Animator animator;

    int speedHash = Animator.StringToHash("Speed");

    [Header("Chase Settings")]
    public float chaseRange = 15f;
    public float stopDistance = 1.5f;

    [Header("Attack Settings")]
    public int damagePerHit = 25;       // damage each hit does
    public float attackCooldown = 1.5f; // time between hits
    private float nextAttackTime = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        agent.stoppingDistance = stopDistance;

        if (player == null)
        {
            Debug.LogError("[GuardNavMesh] player is NOT assigned on " + name);
            return;
        }

        // Try to get Health on player or its parents
        playerHealth = player.GetComponent<Health>();
        if (playerHealth == null)
            playerHealth = player.GetComponentInParent<Health>();

        if (playerHealth == null)
        {
            Debug.LogError("[GuardNavMesh] No Health found on player " + player.name + " or its parents!");
        }
        else
        {
            Debug.Log("[GuardNavMesh] Found Health on player: " + playerHealth.name);
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Chasing
        if (distance <= chaseRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if (agent.hasPath)
                agent.ResetPath();
        }

        // Animation: walk when moving
        float speed = agent.velocity.magnitude;
        animator.SetFloat(speedHash, speed);

        // ATTACK when close enough
        if (distance <= stopDistance)
        {
            if (Time.time >= nextAttackTime)
            {
                Debug.Log("[GuardNavMesh] " + name + " attacking player at distance " + distance);
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void AttackPlayer()
    {
        if (playerHealth == null)
        {
            Debug.LogError("[GuardNavMesh] Tried to attack but playerHealth is NULL!");
            return;
        }

        // Face the player
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        Debug.Log("[GuardNavMesh] Dealing " + damagePerHit + " damage to player");
        playerHealth.TakeDamage(damagePerHit);
    }
}
