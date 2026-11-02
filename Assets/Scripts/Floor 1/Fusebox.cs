using UnityEngine;

// Attach this to the electrical box object.
// Setup: Collider2D (Is Trigger checked).
public class FuseBox : MonoBehaviour, IInteractable
{
    [Tooltip("Shown in a prompt UI, e.g. above the fuse box.")]
    public string interactionPrompt = "Inspect";

    [Header("Conversations")]
    [Tooltip("Plays the first time the player examines the box (the 'Electrical Box' conversation - box is empty, missing three fuses).")]
    public DialogueLine[] boxExplainedLines;

    [Tooltip("Plays once all fuses are collected and the player interacts again (the 'Power Restored' conversation).")]
    public DialogueLine[] powerRestoredLines;

    [Tooltip("Optional: a blocking object (e.g. a locked staircase collider) to disable once power is restored.")]
    public GameObject lockedStaircaseBlocker;

    private bool hasExplainedBox = false;
    private bool powerRestored = false;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        // Already restored - nothing left to say here.
        if (powerRestored) return;

        // All fuses found - trigger the power-restored conversation.
        if (GameState.Instance.AllFusesCollected)
        {
            powerRestored = true;
            DialogueManager.Instance.StartDialogue(powerRestoredLines, OnPowerRestored);
            QuestManager.Instance.SetQuest("Go to the second floor.");
            return;
        }

        // Still missing fuses - explain the puzzle (only the first time).
        if (!hasExplainedBox)
        {
            hasExplainedBox = true;
            DialogueManager.Instance.StartDialogue(boxExplainedLines);
        }
    }

    void OnPowerRestored()
    {
        if (lockedStaircaseBlocker != null)
        {
            lockedStaircaseBlocker.SetActive(false);
        }
    }
}