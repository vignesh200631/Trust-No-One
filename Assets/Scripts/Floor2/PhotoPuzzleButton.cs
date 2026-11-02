using UnityEngine;
using UnityEngine.UI;

public class PhotoPuzzleButton : MonoBehaviour
{
    public int photoID;

    public PhotoPuzzleManager puzzleManager;

    public Image photoImage;

    public void SelectPhoto()
    {
        Debug.Log("Clicked Photo Button: " + gameObject.name);
        Debug.Log("Photo ID: " + photoID);

        if (puzzleManager == null)
        {
            Debug.LogError("Puzzle Manager is missing on: " + gameObject.name);
            return;
        }

        if (photoImage == null)
        {
            Debug.LogError("Photo Image is missing on: " + gameObject.name);
            return;
        }

        puzzleManager.SelectPhoto(photoID, photoImage.sprite);

        gameObject.SetActive(false);
    }
}