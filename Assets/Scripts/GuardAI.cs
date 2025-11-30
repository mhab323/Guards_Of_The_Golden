using UnityEngine;

//[RequireComponent(typeof(GuardHealth))]
public class GuardAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float chaseRange = 15f;
    public float stopDistance = 1.5f;

    void Update()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f; // keep them on the ground

        float dist = dir.magnitude;

        if (dist <= chaseRange)
        {
            // Rotate toward player
            if (dir != Vector3.zero)
                transform.forward = dir.normalized;

            // Move toward player until close enough
            if (dist > stopDistance)
            {
                transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            }
        }
    }
}
