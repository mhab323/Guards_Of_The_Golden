using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBarDual : MonoBehaviour
{
    public Health targetHealth;
    public Slider mainBar;      // green, current HP
    public Slider damageBar;    // red, lags behind
    public float damageLerpSpeed = 5f;

    private int lastHealth;

    void Start()
    {
        if (targetHealth == null) return;

        mainBar.maxValue = targetHealth.maxHealth;
        damageBar.maxValue = targetHealth.maxHealth;

        mainBar.value = targetHealth.currentHealth;
        damageBar.value = targetHealth.currentHealth;

        lastHealth = targetHealth.currentHealth;
    }

    void Update()
    {
        if (targetHealth == null) return;

        // green bar follows health instantly
        mainBar.value = targetHealth.currentHealth;

        // red bar smoothly moves down toward green value
        if (damageBar.value > mainBar.value)
        {
            damageBar.value = Mathf.Lerp(
                damageBar.value,
                mainBar.value,
                damageLerpSpeed * Time.deltaTime
            );
        }
        else
        {
            // if you heal, snap red bar up to match
            damageBar.value = mainBar.value;
        }

        lastHealth = targetHealth.currentHealth;
    }
}
