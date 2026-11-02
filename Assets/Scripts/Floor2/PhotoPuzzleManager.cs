using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotoPuzzleManager : MonoBehaviour
{
    [Header("Puzzle UI")]
    public GameObject puzzlePanel;

    [Header("Available Photo Buttons")]
    public GameObject[] photoButtons;

    [Header("Arrangement Slots")]
    public Image[] arrangementSlots;

    [Header("UI")]
    public TMP_Text resultText;

    [Header("Correct Order")]
    public int[] correctOrder = { 1, 3, 0, 2 };

    [Header("Photo Collection")]
    public int requiredPhotos = 4;

    private List<int> collectedPhotos = new List<int>();
    private List<int> selectedOrder = new List<int>();

    private bool puzzleSolved = false;

    private void Start()
    {
        puzzlePanel.SetActive(false);

        ClearSlots();
    }

    // Called by PhotoCollectible
    public void CollectPhoto(int photoID)
    {
        if (!collectedPhotos.Contains(photoID))
        {
            collectedPhotos.Add(photoID);

            Debug.Log("Photo collected: " + photoID);
            Debug.Log("Total photos: " + collectedPhotos.Count);
            QuestManager.Instance.SetQuest(
            "Find the photographs (" +
            collectedPhotos.Count +
            "/" +
            requiredPhotos +
        ")"
        );
        }
    }

    public bool HasAllPhotos()
    {
        return collectedPhotos.Count >= requiredPhotos;
    }

    public void OpenPuzzle()
    {
        if (!HasAllPhotos())
        {
            Debug.Log("Find all photographs first.");
            return;
        }

        if (puzzleSolved)
        {
            puzzlePanel.SetActive(true);
            return;
        }

        puzzlePanel.SetActive(true);

        ResetPuzzleState();
    }

    public void SelectPhoto(int photoID, Sprite photoSprite)
    {
        if (puzzleSolved)
            return;

        if (selectedOrder.Contains(photoID))
            return;

        if (selectedOrder.Count >= arrangementSlots.Length)
            return;

        selectedOrder.Add(photoID);

        int slotIndex = selectedOrder.Count - 1;

        if (arrangementSlots[slotIndex] == null)
        {
            Debug.LogError("Arrangement Slot " + (slotIndex + 1) + " is not assigned!");
            return;
        }

        arrangementSlots[slotIndex].sprite = photoSprite;
        arrangementSlots[slotIndex].enabled = true;
    }

    public void CheckOrder()
    {
        if (selectedOrder.Count < correctOrder.Length)
        {
            resultText.text = "Arrange all photographs first.";
            return;
        }

        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (selectedOrder[i] != correctOrder[i])
            {
                resultText.text = "That doesn't seem right.";
                return;
            }
        }

        PuzzleSolved();
    }

    private void PuzzleSolved()
    {
        puzzleSolved = true;

        resultText.text = "Correct order!\nThe locker code is: 4279";

        Debug.Log("Photo puzzle solved!");

        QuestManager.Instance.SetQuest("Use code to unlock locker");
    }

    // Called by Reset Button
    public void ResetPuzzle()
    {
        if (puzzleSolved)
            return;

        ResetPuzzleState();
    }

    private void ResetPuzzleState()
    {
        selectedOrder.Clear();

        ClearSlots();

        EnableAllPhotoButtons();

        resultText.text = "";
    }

    private void ClearSlots()
    {
        foreach (Image slot in arrangementSlots)
        {
            if (slot != null)
            {
                slot.sprite = null;
                slot.enabled = false;
            }
        }
    }

    private void EnableAllPhotoButtons()
    {
        foreach (GameObject photoButton in photoButtons)
        {
            if (photoButton != null)
            {
                photoButton.SetActive(true);
            }
        }
    }

    // Called by Close Button
    public void ClosePuzzle()
    {
        if (!puzzleSolved)
        {
            ResetPuzzleState();
        }

        puzzlePanel.SetActive(false);
    }
}