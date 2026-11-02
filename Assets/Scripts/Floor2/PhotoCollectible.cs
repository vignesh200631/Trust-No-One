using UnityEngine;

public class PhotoCollectible : MonoBehaviour, IInteractable
{
    public string interactionPrompt = "Take Photograph";

    [Header("Photo ID")]
    public int photoID;

    [Header("Puzzle Manager")]
    public PhotoPuzzleManager puzzleManager;

    public string InteractionPrompt => interactionPrompt;

    public void Interact()
    {
        puzzleManager.CollectPhoto(photoID);

        gameObject.SetActive(false);

        if(puzzleManager.HasAllPhotos())
        {
            QuestManager.Instance.SetQuest("Go to table and arrange them");
        }
    }
}