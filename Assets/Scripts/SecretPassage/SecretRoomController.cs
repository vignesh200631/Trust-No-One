using UnityEngine;
using System.Collections;

public class SecretRoomController : MonoBehaviour
{
    [Header("Player")]
    public PlayerController playerController;
    public Transform playerTransform;

    [Header("YES Path Dialogue")]
    public DialogueLine[] yesLines;

    [Header("NO Path Dialogue")]
    public DialogueLine[] noLines;

    [Header("Stranger / Monster")]
    public Transform strangerTransform;
    public SpriteRenderer strangerRenderer;

    [Header("Monster Transformation")]
    public Color monsterColor = Color.red;

    [Header("Animator Controllers")]
    public Animator strangerAnimator;
    public RuntimeAnimatorController strangerAnimatorController;
    public RuntimeAnimatorController vampireAnimatorController;

    [Header("YES Path")]
    public float yesMoveSpeed = 1f;
    public float stopDistance = 1.2f;

    [Header("YES Path Attack")]
    public float attackDelay = 0.3f;
    public float attackDuration = 1f;

    [Header("NO Path")]
    public MonsterChase monsterChase;

    [Header("Ending Fade")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 3f;

    // =========================
    // YES PATH
    // =========================

    public void StartYesPath()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        DialogueManager.Instance.StartDialogue(yesLines, () =>
        {
            StartCoroutine(YesEndingSequence());
        });
    }

    private IEnumerator YesEndingSequence()
    {
        yield return new WaitForSeconds(0.5f);

        // =========================
        // TRANSFORM TO VAMPIRE
        // =========================

        TransformToVampire();

        // Make sure vampire starts idle
        if (strangerAnimator != null)
        {
            strangerAnimator.SetFloat("Speed", 0f);
        }

        // Give player time to see transformation
        yield return new WaitForSeconds(0.8f);

        // =========================
        // WALK TOWARD PLAYER
        // =========================

        if (strangerAnimator != null)
        {
            strangerAnimator.SetFloat("Speed", 1f);
        }

        while (
            strangerTransform != null &&
            playerTransform != null &&
            Vector2.Distance(
                strangerTransform.position,
                playerTransform.position
            ) > stopDistance
        )
        {
            strangerTransform.position = Vector2.MoveTowards(
                strangerTransform.position,
                playerTransform.position,
                yesMoveSpeed * Time.deltaTime
            );

            FlipVampireTowardsPlayer();

            yield return null;
        }

        // =========================
        // STOP MOVING
        // =========================

        if (strangerAnimator != null)
        {
            strangerAnimator.SetFloat("Speed", 0f);
        }

        yield return new WaitForSeconds(attackDelay);

        // =========================
        // ATTACK PLAYER
        // =========================

        if (strangerAnimator != null)
        {
            strangerAnimator.SetTrigger("Attack");
        }

        Debug.Log("VAMPIRE ATTACK - YES ENDING");

        // Wait until attack animation finishes
        yield return new WaitForSeconds(attackDuration);

        // =========================
        // FADE TO BLACK
        // =========================

        yield return StartCoroutine(FadeToBlack());

        Debug.Log("YES ENDING COMPLETE");
    }

    private void FlipVampireTowardsPlayer()
    {
        if (strangerTransform == null || playerTransform == null)
            return;

        Vector3 scale = strangerTransform.localScale;

        if (playerTransform.position.x < strangerTransform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else if (playerTransform.position.x > strangerTransform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }

        strangerTransform.localScale = scale;
    }


    // =========================
    // NO PATH
    // =========================

    public void StartNoPath()
    {
        Debug.Log("Player chose NO");

        // Keep player frozen during dialogue
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        DialogueManager.Instance.StartDialogue(noLines, () =>
        {
            StartCoroutine(NoPathSequence());
        });
    }

    private IEnumerator NoPathSequence()
    {
        yield return new WaitForSeconds(0.5f);

        TransformToVampire();

        QuestManager.Instance.SetQuest("RUN");

        yield return new WaitForSeconds(1f);

        // Enable player movement
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Start monster chase
        if (monsterChase != null)
        {
            monsterChase.StartChase();
        }
        else
        {
            Debug.LogError("MonsterChase is not assigned!");
        }
    }


    // =========================
    // FADE
    // =========================

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

    private void TransformToVampire()
    {
        if (strangerAnimator == null)
        {
            Debug.LogError("Stranger Animator is not assigned!");
            return;
        }

        if (vampireAnimatorController == null)
        {
            Debug.LogError("Vampire Animator Controller is not assigned!");
            return;
        }

        // Change Stranger Animator Controller to Vampire Animator Controller
        strangerAnimator.runtimeAnimatorController = vampireAnimatorController;

        // Reset Vampire Animator to its default state
        strangerAnimator.Rebind();
        strangerAnimator.Update(0f);

        Debug.Log("Transformation complete: Stranger → Vampire");
    }

    private void OnEnable()
    {
        if (monsterChase != null)
        {
            monsterChase.OnPlayerCaught += StartNoEnding;
        }
    }

    private void OnDisable()
    {
        if (monsterChase != null)
        {
            monsterChase.OnPlayerCaught -= StartNoEnding;
        }
    }

    private void StartNoEnding()
    {
        StartCoroutine(NoEndingSequence());
    }

    private IEnumerator NoEndingSequence()
    {
        // Freeze player
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Stop vampire
        if (monsterChase != null)
        {
            monsterChase.StopChase();
        }

        yield return new WaitForSeconds(0.3f);

    // Fade screen
        yield return StartCoroutine(FadeToBlack());

        Debug.Log("NO ENDING COMPLETE");
    }
    
}