using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBarFancy : MonoBehaviour
{
    public Health targetHealth;

    [Header("Sliders")]
    public Slider mainBar;      // green, current HP
    public Slider damageBar;    // red, lags behind

    [Header("Blink settings")]
    public Image mainFillImage; // Fill image from MainBar
    public Color normalColor = Color.green;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Damage bar settings")]
    public float damageLerpSpeed = 5f; // how fast red tail catches up

    private int lastHealth;
    private float flashTimer;

    void Start()
    {
        if (targetHealth == null) return;

        mainBar.maxValue = targetHealth.maxHealth;
        damageBar.maxValue = targetHealth.maxHealth;

        mainBar.value = targetHealth.currentHealth;
        damageBar.value = targetHealth.currentHealth;

        lastHealth = targetHealth.currentHealth;

        if (mainFillImage != null)
            mainFillImage.color = normalColor;
    }

    void Update()
    {
        if (targetHealth == null) return;

        int current = targetHealth.currentHealth;

        // main (green) bar = current HP
        mainBar.value = current;

        // if we took damage this frame
        if (current < lastHealth)
        {
            // start red bar from previous HP (so only lost part is visible)
            damageBar.value = lastHealth;

            // start blink
            flashTimer = flashDuration;
            if (mainFillImage != null)
                mainFillImage.color = flashColor;
        }

        // make red bar slide down towards green
        if (damageBar.value > mainBar.value)
        {
            damageBar.value = Mathf.MoveTowards(
                damageBar.value,
                mainBar.value,
                damageLerpSpeed * Time.deltaTime * targetHealth.maxHealth
            );
        }
        else
        {
            damageBar.value = mainBar.value;
        }

        // handle blink timer
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f && mainFillImage != null)
                mainFillImage.color = normalColor;
        }

        lastHealth = current;
    }
}
