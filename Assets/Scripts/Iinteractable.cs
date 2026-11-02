
public interface IInteractable
{
    // Optional label shown in a prompt UI, e.g. "Talk" or "Pick up".
    string InteractionPrompt { get; }

    // Called by PlayerInteractor when the player presses the interact key
    // while this object is the current target.
    void Interact();
}