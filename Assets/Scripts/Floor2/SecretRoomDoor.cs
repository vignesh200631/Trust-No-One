using UnityEngine;

public class SecretRoomDoor : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    public string interactionPrompt = "Unlock Door";

    [Header("Key")]
    public SecretRoomKey secretRoomKey;

    [Header("Door")]
    public GameObject doorObject;

    private bool isUnlocked = false;

    public string InteractionPrompt
    {
        get
        {
            if (isUnlocked)
                return "";

            return interactionPrompt;
        }
    }

    public void Interact()
    {
        if (isUnlocked)
            return;

        if (secretRoomKey != null && secretRoomKey.HasKey)
        {
            UnlockDoor();
        }
        else
        {
            Debug.Log("The door is locked. Need a key.");
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;

        Debug.Log("Secret Room Door Unlocked!");

        if (doorObject != null)
        {
            doorObject.SetActive(false);
        }
    }
}