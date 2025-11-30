using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public int damage = 25;
    public string enemyTag = "Guard";
    public bool canHit = true;   // later you can turn this on only during swing

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sword trigger with: " + other.name);

        if (!canHit) return;
        if (!other.CompareTag(enemyTag)) return;

        Health h = other.GetComponent<Health>();
        if (h == null)
            h = other.GetComponentInParent<Health>();

        if (h != null)
        {
            Debug.Log("Hit " + other.name + " for " + damage);
            h.TakeDamage(damage);
        }
    }
}
