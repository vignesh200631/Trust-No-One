using UnityEngine;
using TMPro;

public class LockerInteraction : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    public string interactionPrompt = "Inspect";

    [Header("Locker Code")]
    public string correctCode = "4279";

    [Header("Keypad UI")]
    public GameObject keypadPanel;
    public TMP_InputField codeInput;
    public TMP_Text messageText;

    [Header("Locker Content")]
    public GameObject secretRoomKey;

    private bool isOpened = false;
    private bool hasBeenInspected = false;

    public bool HasBeenInspected => hasBeenInspected;
    public bool IsOpened => isOpened;

    public string InteractionPrompt => interactionPrompt;

    private void Start()
    {
        if (keypadPanel != null)
            keypadPanel.SetActive(false);

        if (secretRoomKey != null)
            secretRoomKey.SetActive(false);
    }

    public void Interact()
    {
        if (isOpened)
        {
            return;
        }

        hasBeenInspected = true;
        OpenKeypad();
    }

    private void OpenKeypad()
    {
        keypadPanel.SetActive(true);

        codeInput.text = "";
        messageText.text = "";

        codeInput.Select();
        codeInput.ActivateInputField();
    }

    public void CheckCode()
    {
        string enteredCode = codeInput.text;

        if (enteredCode == correctCode)
        {
            UnlockLocker();
        }
        else
        {
            messageText.text = "Incorrect code.";
            codeInput.text = "";
            codeInput.ActivateInputField();
        }
    }

    private void UnlockLocker()
    {
        isOpened = true;

        messageText.text = "Unlocked!";

        Debug.Log("Locker opened!");

        Invoke(nameof(RevealKey), 0.5f);
    }

    private void RevealKey()
    {
        keypadPanel.SetActive(false);

        if (secretRoomKey != null)
        {
            secretRoomKey.SetActive(true);
        }
    }

    public void CloseKeypad()
    {
        keypadPanel.SetActive(false);
    }
}