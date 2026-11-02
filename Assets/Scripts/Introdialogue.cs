using UnityEngine;

public class IntroDialogue : MonoBehaviour
{
    [Header("Intro Dialogue")]
    public DialogueLine[] introLines;

    [Header("Player Controller")]
    public PlayerController playerController;

    [Header("Optional Trust Choice")]
    public TrustChoiceManager trustChoiceManager;

    void Start()
    {
        if (introLines == null || introLines.Length == 0) return;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        DialogueManager.Instance.StartDialogue(introLines, () =>
        {
            if (trustChoiceManager != null)
            {
                trustChoiceManager.ShowChoice();
            }
            else if (playerController != null)
            {
                playerController.enabled = true;
            }
        });
    }
}