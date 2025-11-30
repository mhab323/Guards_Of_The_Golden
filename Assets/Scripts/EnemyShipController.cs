using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    public Transform player;          // your main ship

    public float distanceAhead = 40f;     // how far in front of player
    public float lateralAmplitude = 20f;  // how far “sideways”
    public float lateralSpeed = 1f;       // how fast they move
    public float heightOffset = 0f;       // water height offset
    public float phaseOffset = 0f;        // different per enemy (0, 3.14, etc.)

    void Update()
    {
        if (player == null) return;

        // FRONT = player.right (red arrow)
        Vector3 frontDir = player.right;
        // SIDE  = player.forward (blue arrow)
        Vector3 sideDir = player.forward;

        // position in front of player
        Vector3 basePos = player.position + frontDir * distanceAhead;
        basePos.y += heightOffset;

        // wiggle left/right relative to ship’s front
        float x = Mathf.Sin(Time.time * lateralSpeed + phaseOffset) * lateralAmplitude;
        Vector3 targetPos = basePos + sideDir * x;

        // snap there each frame
        transform.position = targetPos;

        // face the player
        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
    }
}
