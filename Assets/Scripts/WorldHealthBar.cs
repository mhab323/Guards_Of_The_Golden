using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldHealthBar : MonoBehaviour
{
    public Health targetHealth;      // Guard's Health component
    public Slider slider;           // The Slider
    public Image fillImage;         // The "Fill" image of the slider

    public Color normalColor = Color.green;
    public Color damageColor = Color.red;
    public float damageFlashTime = 0.15f;

    private int lastHealth;

    void Start()
    {
        if (targetHealth == null || slider == null) return;

        slider.maxValue = targetHealth.maxHealth;
        slider.value = targetHealth.currentHealth;
        lastHealth = targetHealth.currentHealth;

        if (fillImage != null)
            fillImage.color = normalColor;
    }

    void Update()
    {
        if (targetHealth == null || slider == null) return;

        // update value
        slider.value = targetHealth.currentHealth;

        // detect damage
        if (targetHealth.currentHealth < lastHealth)
        {
            // took damage
            StartCoroutine(FlashDamage());
        }

        lastHealth = targetHealth.currentHealth;
    }

    IEnumerator FlashDamage()
    {
        if (fillImage == null) yield break;

        fillImage.color = damageColor;        // turn red
        yield return new WaitForSeconds(damageFlashTime);
        fillImage.color = normalColor;        // back to green
    }
}
