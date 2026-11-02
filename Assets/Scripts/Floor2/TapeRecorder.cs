using UnityEngine;

public class TapeRecorder : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    public string interactionPrompt = "Play";

    [Header("Dialogue")]
    public DialogueLine[] recordingLines;

    private bool hasPlayed = false;

    public bool HasPlayed => hasPlayed;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        if (hasPlayed) return;

        hasPlayed = true;

        DialogueManager.Instance.StartDialogue(recordingLines);

        QuestManager.Instance.SetQuest("Talk to Stranger");
    }
}