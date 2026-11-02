using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI")]
    public GameObject questPanel;
    public TextMeshProUGUI objectiveText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetQuest(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = newObjective;
        }

        if (questPanel != null)
        {
            questPanel.SetActive(true);
        }

        Debug.Log("New Objective: " + newObjective);
    }

    public void HideQuest()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }
}