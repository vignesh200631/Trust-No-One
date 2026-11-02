using UnityEngine;

public class PhotoPuzzleBoard : MonoBehaviour, IInteractable
{
    public string interactionPrompt = "Examine";
    public PhotoPuzzleManager puzzleManager;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        puzzleManager.OpenPuzzle();
    }
}