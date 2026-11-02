using UnityEngine;
using UnityEngine.SceneManagement;

// Attach this to a trigger zone placed at the top of the staircase
// (or wherever the player should walk to move to the next floor).
// Setup: Collider2D with "Is Trigger" checked.
//
// This is separate from the FuseBox's "lockedStaircaseBlocker" object -
// that blocker physically stops the player from walking up until the
// power is restored. Once it's disabled, the player can actually reach
// this trigger and move to the next scene.
public class FloorTransition : MonoBehaviour
{
    [Tooltip("Exact name of the scene to load (must be added in File > Build Settings > Scenes In Build).")]
    public string nextSceneName;

    [Tooltip("Tag used by the Player GameObject. Default is 'Player'.")]
    public string playerTag = "Player";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("FloorTransition: no scene name set.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}