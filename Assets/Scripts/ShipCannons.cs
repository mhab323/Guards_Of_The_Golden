using UnityEngine;

public class ShipCannons : MonoBehaviour
{
    public Transform[] muzzles;          // left/right cannon tips
    public GameObject cannonballPrefab;  // cannonball prefab
    public float shootForce = 30f;
    public float fireCooldown = 0.3f;

    [Header("Audio")]
    public AudioSource audioSource;      // AudioSource on the ship
    public AudioClip cannonFireClip;     // your cannon sound

    private float lastShotTime;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastShotTime + fireCooldown)
        {
            FireCannons();
            lastShotTime = Time.time;
        }
    }

    void FireCannons()
    {
        // spawn cannonballs
        foreach (Transform muzzle in muzzles)
        {
            GameObject ball = Instantiate(cannonballPrefab, muzzle.position, muzzle.rotation);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.AddForce(muzzle.forward * shootForce, ForceMode.Impulse);
            Destroy(ball, 5f);
        }

        // play sound once when firing
        if (audioSource != null && cannonFireClip != null)
        {
            audioSource.PlayOneShot(cannonFireClip);
        }
    }
}
