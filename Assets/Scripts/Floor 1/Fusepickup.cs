using UnityEngine;

// Attach this to each of the 3 fuse objects in the scene.
// Setup: Collider2D (Is Trigger checked), Sprite Renderer for the fuse icon.
public class FusePickup : MonoBehaviour, IInteractable
{
    [Tooltip("Shown in a prompt UI, e.g. above the fuse.")]
    public string interactionPrompt = "Pick up";

    //[Tooltip("The conversation that plays when this fuse is picked up (e.g. the 'First Fuse' / 'Second Fuse' / 'Third Fuse' lines).")]
    //public DialogueLine[] pickupLines;

    private bool collected = false;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        if (collected) return;
        collected = true;

        // Hide and disable immediately so the player can't trigger it twice
        // while the dialogue is still playing.
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        GameState.Instance.CollectFuse();

        //DialogueManager.Instance.StartDialogue(pickupLines);
    }
}