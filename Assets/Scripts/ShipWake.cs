using UnityEngine;

public class ShipWake : MonoBehaviour
{
    [Header("Refs")]
    public Transform ship;                // ship transform
    public ParticleSystem wakeParticles;  // particle system at the back

    [Header("Speed → Emission")]
    public float minSpeed = 0.5f;         // speed at which wake starts
    public float maxSpeed = 10f;          // speed at which wake is max
    public float maxEmission = 60f;       // max particles per second

    private Vector3 lastPos;

    void Start()
    {
        if (ship == null)
            ship = transform; // default: this object

        if (wakeParticles == null)
            wakeParticles = GetComponentInChildren<ParticleSystem>();

        lastPos = ship.position;
    }

    void Update()
    {
        // Calculate speed from movement since last frame
        Vector3 delta = ship.position - lastPos;
        float speed = delta.magnitude / Time.deltaTime;
        lastPos = ship.position;

        var emission = wakeParticles.emission;

        if (speed < minSpeed)
        {
            emission.rateOverTime = 0f;
            return;
        }

        float t = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
        emission.rateOverTime = Mathf.Lerp(0f, maxEmission, t);
    }
}
