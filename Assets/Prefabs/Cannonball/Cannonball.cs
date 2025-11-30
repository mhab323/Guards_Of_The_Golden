using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public GameObject explosionPrefab; // assign in inspector

    private void OnCollisionEnter(Collision collision)
    {
        // Spawn explosion at hit point
        if (explosionPrefab != null)
        {
            // Try to use the first contact point if available
            Vector3 pos = transform.position;
            if (collision.contactCount > 0)
                pos = collision.GetContact(0).point;

            Instantiate(explosionPrefab, pos, Quaternion.identity);
        }

        // Destroy enemy if tagged
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        // Destroy cannonball on any hit
        Destroy(gameObject);
    }
}
