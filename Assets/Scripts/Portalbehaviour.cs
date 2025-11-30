using UnityEngine;
using UnityEngine.SceneManagement;

public class Portalbehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        // If we're past the last scene, go back to 0
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
            nextScene = 0;

        SceneManager.LoadScene(nextScene);
    }
}
