using UnityEngine;

// Attach this to the Stranger GameObject, alongside a Collider2D (Is Trigger checked).
public class StrangerInteraction : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [Tooltip("Shown in a prompt UI, e.g. above the Stranger's head.")]
    public string interactionPrompt = "Talk";

    [Tooltip("Lines shown the very first time the player talks to him.")]
    public DialogueLine[] firstMeetingLines;

    [Tooltip("Lines shown on repeat interactions (idle chatter / hints).")]
    public DialogueLine[] repeatLines;

    private bool hasMetPlayer = false;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        if (!hasMetPlayer)
        {
            hasMetPlayer = true;
            PlayLines(firstMeetingLines);
        }
        else
        {
            PlayLines(repeatLines);
        }
    }

    void PlayLines(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        DialogueManager.Instance.StartDialogue(lines);

        QuestManager.Instance.SetQuest("Restore the power.");
    }
}