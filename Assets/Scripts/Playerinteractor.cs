using UnityEngine;

// Attach this to the Player GameObject.
// Requires a Collider2D set to "Is Trigger" (a small circle around the player
// works well - this defines the player's interaction range).
public class PlayerInteractor : MonoBehaviour
{
    [Tooltip("Key used to interact with whatever is currently in range.")]
    public KeyCode interactKey = KeyCode.E;

    private IInteractable currentTarget;

    void Update()
    {
        // Don't fire interactions while a dialogue is already open -
        // otherwise the same key press advances dialogue AND re-triggers
        // Interact() on whatever is still in range (e.g. the Stranger).
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueOpen)
        {
            return;
        }

        if (currentTarget != null && Input.GetKeyDown(interactKey))
        {
            currentTarget.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // GetComponent<IInteractable>() works fine even though it's an
        // interface, not a MonoBehaviour - Unity finds whichever script
        // on that object implements it.
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentTarget = interactable;

            // Hook a UI prompt here later, e.g.:
            // PromptUI.Show(interactable.InteractionPrompt);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentTarget)
        {
            currentTarget = null;

            // PromptUI.Hide();
        }
    }
}