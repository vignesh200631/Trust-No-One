using UnityEngine;
using System.Collections;

public class MonsterCatch : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Chase")]
    public MonsterChase monsterChase;

    [Header("Reset Positions")]
    public Transform playerChaseStart;
    public Transform monsterChaseStart;

    [Header("Player Controller")]
    public PlayerController playerController;

    private bool isResetting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isResetting)
            return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(RestartChase());
        }
    }

    private IEnumerator RestartChase()
    {
        isResetting = true;

    // Stop vampire movement
        if (monsterChase != null)
        {
            monsterChase.Attack();
        }

        // Freeze player
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Debug.Log("PLAYER CAUGHT!");

        // Wait for vampire attack animation
        yield return new WaitForSeconds(1f);

        // Reset player position
        if (player != null && playerChaseStart != null)
        {
            player.position = playerChaseStart.position;
        }

        // Reset vampire position
        if (monsterChaseStart != null)
        {
            transform.position = monsterChaseStart.position;
        }

        yield return new WaitForSeconds(0.5f);

        // Enable player
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Restart chase
        if (monsterChase != null)
        {
            monsterChase.StartChase();
        }

        isResetting = false;
    }
}