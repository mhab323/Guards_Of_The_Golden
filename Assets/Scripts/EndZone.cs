using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    [Header("Scene")]
    public string endSceneName = "EndScene";   // set in Inspector

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[EndZone] Trigger ENTER by: " + other.name + " | tag: " + other.tag);

        // OPTION 1: tag check only
        if (!other.CompareTag("Player"))
        {
            Debug.Log("[EndZone] Not the Player, ignoring.");
            return;
        }

        if (string.IsNullOrEmpty(endSceneName))
        {
            Debug.LogError("[EndZone] endSceneName is EMPTY! Set it in the Inspector.");
            return;
        }

        Debug.Log("[EndZone] Loading scene: " + endSceneName);
        SceneManager.LoadScene(endSceneName);
    }
}
