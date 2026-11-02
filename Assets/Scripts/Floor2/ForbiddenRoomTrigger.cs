using UnityEngine;

public class ForbiddenRoomTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueLine[] warningLines;

    [Header("Player")]
    public PlayerController playerController;
    public string playerTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        DialogueManager.Instance.StartDialogue(warningLines, () =>
        {
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        });

        QuestManager.Instance.SetQuest("Investigate the forbidden room.");
    }
}