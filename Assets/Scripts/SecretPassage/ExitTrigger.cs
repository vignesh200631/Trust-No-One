using UnityEngine;
using System.Collections;

public class ExitTrigger : MonoBehaviour
{
    [Header("Monster")]
    public MonsterChase monsterChase;

    [Header("Player")]
    public PlayerController playerController;

    [Header("Ending")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 3f;

    private bool exitTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exitTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            exitTriggered = true;

            StartCoroutine(ExitSequence());
        }
    }

    private IEnumerator ExitSequence()
    {
        Debug.Log("PLAYER REACHED EXIT!");

        // Stop monster before exit
        if (monsterChase != null)
        {
            monsterChase.StopAtExit();
        }

        // Short moment so player sees monster stop
        yield return new WaitForSeconds(1f);

        // Freeze player
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        Debug.Log("GOOD ENDING COMPLETE!");
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeCanvasGroup == null)
            yield break;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            fadeCanvasGroup.alpha = Mathf.Lerp(
                0f,
                1f,
                elapsedTime / fadeDuration
            );

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }
}