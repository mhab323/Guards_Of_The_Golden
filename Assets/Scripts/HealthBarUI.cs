using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health targetHealth;   // player health
    public Slider slider;         // HP slider

    void Start()
    {
        if (targetHealth != null && slider != null)
        {
            slider.maxValue = targetHealth.maxHealth;
            slider.value = targetHealth.currentHealth;
        }
    }

    void Update()
    {
        if (targetHealth != null && slider != null)
        {
            slider.value = targetHealth.currentHealth;
        }
    }
}
