using UnityEngine;

public class GuardBehaviour : MonoBehaviour
{
    [Header("References")]
    public GameObject player;      // drag Player here in Inspector

    Animator animator;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;   // how fast the guard runs
    public float chaseRange = 15f; // how far they can see you
    public float stopDistance = 1.5f; // how close they get before stopping

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null || player == null) return;

        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0f; // stay on ground
        float distance = dir.magnitude;

        if (distance <= chaseRange)
        {
            // Rotate toward player
            if (dir != Vector3.zero)
                transform.forward = dir.normalized;

            // Move toward player until close enough
            if (distance > stopDistance)
            {
                transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            }

            // 1 = walk/run
            if (animator.GetInteger("State") != 1)
                animator.SetInteger("State", 1);
        }
        else
        {
            // 0 = idle
            if (animator.GetInteger("State") != 0)
                animator.SetInteger("State", 0);
        }
    }
}
