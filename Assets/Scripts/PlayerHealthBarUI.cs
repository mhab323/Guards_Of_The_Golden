using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    public Health playerHealth;  // Player's Health
    public Slider slider;        // The UI Slider

    void Start()
    {
        if (playerHealth == null)
        {
            // try to auto-find player by tag if not set
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerHealth = playerObj.GetComponent<Health>();
        }

        if (playerHealth == null || slider == null)
        {
            Debug.LogError("[PlayerHealthBarUI] Missing references!");
            return;
        }

        slider.minValue = 0;
        slider.maxValue = playerHealth.maxHealth;
        slider.value = playerHealth.currentHealth;
    }

    void Update()
    {
        if (playerHealth == null || slider == null) return;

        slider.value = playerHealth.currentHealth;
    }
}
