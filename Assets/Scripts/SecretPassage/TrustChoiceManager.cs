using UnityEngine;

public class TrustChoiceManager : MonoBehaviour
{
    [Header("Choice UI")]
    public GameObject trustChoicePanel;

    [Header("Secret Room Controller")]
    public SecretRoomController secretRoomController;

    private bool choiceMade = false;

    private void Start()
    {
        trustChoicePanel.SetActive(false);
    }

    public void ShowChoice()
    {
        if (choiceMade)
            return;

        trustChoicePanel.SetActive(true);
    }

    public void ChooseYes()
    {
        if (choiceMade)
            return;

        choiceMade = true;

        trustChoicePanel.SetActive(false);

        secretRoomController.StartYesPath();
    }

    public void ChooseNo()
    {
        if (choiceMade)
            return;

        choiceMade = true;

        trustChoicePanel.SetActive(false);

        secretRoomController.StartNoPath();
    }
}