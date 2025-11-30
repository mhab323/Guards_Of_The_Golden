using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public UnityEvent onDeath;   // you can hook this in Inspector

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return; // already dead

        currentHealth -= amount;
        Debug.Log("[PLAYER HEALTH] " + name + " took " + amount + " dmg. HP = " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        onDeath?.Invoke();

        if (CompareTag("Guard"))
        {
            Destroy(gameObject); // only guards die like this
        }
        else if (CompareTag("Player"))
        {
            // trigger game over / respawn here instead of Destroy
        }
    }
}
