using UnityEngine;

public class SecretRoomKey : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    public string interactionPrompt = "Take Key";

    private bool hasKey = false;

    public bool HasKey => hasKey;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        if (hasKey)
            return;

        hasKey = true;

        Debug.Log("Secret Room Key collected!");

        gameObject.SetActive(false);

        QuestManager.Instance.SetQuest("Talk to Stranger about key");
    }
}