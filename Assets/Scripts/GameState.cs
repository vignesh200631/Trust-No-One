using UnityEngine;

// A single place to track progress that multiple objects need to know about -
// e.g. the FuseBox needs to know how many fuses have been picked up, even
// though each fuse is a separate object in the scene.
public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("Floor 1 - Fuses")]
    public int fusesCollected = 0;
    public int fusesRequired = 3;

    public bool AllFusesCollected => fusesCollected >= fusesRequired;

    void Awake()
    {
        // Persist across scene loads (Floor 1 -> Floor 2) so progress isn't
        // lost, and make sure only one GameState ever exists.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CollectFuse()
    {
        fusesCollected++;

        QuestManager.Instance.SetQuest(
            "Find the fuses (" +
            fusesCollected +
            "/" +
            fusesRequired +
        ")"
        );
    }
}