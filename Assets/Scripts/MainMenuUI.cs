using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Called by the New Game button
    public void NewGame()
    {
        // Load your first gameplay scene.
        // If MainMenu is buildIndex 0, level1 is usually 1:
        SceneManager.LoadScene(1);

        // OR load by name:
        // SceneManager.LoadScene("Level1");
    }

    // Called by the Exit button
    public void ExitGame()
    {
        Debug.Log("Exit Game pressed");
        Application.Quit();

        // This makes it work in the editor as well:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
