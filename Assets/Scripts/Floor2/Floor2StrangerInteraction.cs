using UnityEngine;

public class Floor2StrangerInteraction : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    public string interactionPrompt = "Talk";

    [Header("Required Objects")]
    public TapeRecorder tapeRecorder;
    public LockerInteraction locker;
    public SecretRoomKey secretRoomKey;

    [Header("Dialogue Before Investigation")]
    public DialogueLine[] beforeInvestigationLines;

    [Header("Dialogue After Investigation")]
    public DialogueLine[] afterInvestigationLines;

    [Header("Dialogue After Key Found")]
    public DialogueLine[] keyFoundLines;

    [Header("Repeat Dialogue")]
    public DialogueLine[] repeatLines;

    [Header("Repeat Dialogue After Key")]
    public DialogueLine[] afterKeyRepeatLines;

    private bool hasGivenPhotoHint = false;
    private bool hasTalkedAfterKey = false;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        // KEY HAS BEEN COLLECTED
        if (secretRoomKey != null && secretRoomKey.HasKey)
        {
            if (!hasTalkedAfterKey)
            {
                hasTalkedAfterKey = true;

                DialogueManager.Instance.StartDialogue(keyFoundLines);

                QuestManager.Instance.SetQuest("Open the Secret Passage");
            }
            else
            {
                DialogueManager.Instance.StartDialogue(afterKeyRepeatLines);
            }

            return;
        }

        // PLAYER FOUND TAPE AND LOCKER
        if (tapeRecorder.HasPlayed &&
            !hasGivenPhotoHint)
        {
            hasGivenPhotoHint = true;

            DialogueManager.Instance.StartDialogue(afterInvestigationLines);
            QuestManager.Instance.SetQuest("Find four photographs");
            return;
        }

        // AFTER PHOTO HINT
        if (hasGivenPhotoHint)
        {
            DialogueManager.Instance.StartDialogue(repeatLines);
            return;
        }

        // BEFORE INVESTIGATION
        DialogueManager.Instance.StartDialogue(beforeInvestigationLines);
    }
}