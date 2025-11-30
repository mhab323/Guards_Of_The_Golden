using UnityEngine;
using System.Collections;

public class NPCDrunkStop : MonoBehaviour
{
    // === Timing Settings ===
    public float walkDuration = 5.0f; // How long to walk before stopping

    // === Private Reference ===
    private Animator _animator;

    // --- Animator Parameter Hash ---
    private int SpeedHash;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator component not found. Cannot run animation.");
            enabled = false;
            return;
        }

        SpeedHash = Animator.StringToHash("Speed");

        // Start the single action sequence
        StartCoroutine(WalkAndStop());
    }

    IEnumerator WalkAndStop()
    {
        // ===================================
        // 1. START WALKING
        // Triggers Entry -> Drunk Walking Turn
        // ===================================

        // Set Speed to 1.0 to meet the Drunk Walking Turn start condition
        _animator.SetFloat(SpeedHash, 1.0f);
        Debug.Log("NPC State: Walking (via Root Motion)");

        yield return new WaitForSeconds(walkDuration);


        // ===================================
        // 2. STOP AND GO IDLE
        // Triggers Drunk Walking Turn -> Idle (Condition: Speed < 0.1)
        // ===================================

        // Set Speed to 0.0 to stop movement and transition to Idle
        _animator.SetFloat(SpeedHash, 0.0f);
        Debug.Log("NPC State: Idle");

        // Script ends here. The NPC should now be idle at its final position.
    }
}