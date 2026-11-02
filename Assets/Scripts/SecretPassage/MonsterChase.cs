using System;
using System.Collections;
using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Chase Settings")]
    public float chaseSpeed = 3f;

    [Header("Exit Settings")]
    public Transform exitStopPoint;

    [Header("Vampire Animation")]
    public Animator vampireAnimator;

    [Header("Attack Settings")]
    public float attackDuration = 1f;

    public event Action OnPlayerCaught;

    private bool isChasing = false;
    private bool isAttacking = false;

    private void Update()
    {
        if (!isChasing || player == null || isAttacking)
            return;

        // Chase player
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );

        // Play movement animation
        if (vampireAnimator != null)
        {
            vampireAnimator.SetFloat("Speed", 1f);
        }

        FlipTowardsPlayer();
    }

    public void StartChase()
    {
        isChasing = true;
        isAttacking = false;

        Debug.Log("CHASE STARTED!");
    }

    public void StopChase()
    {
        isChasing = false;

        if (vampireAnimator != null)
        {
            vampireAnimator.SetFloat("Speed", 0f);
        }

        Debug.Log("CHASE STOPPED!");
    }

    public void Attack()
    {
        StopChase();

        if (vampireAnimator != null)
        {
            vampireAnimator.SetFloat("Speed", 0f);
            vampireAnimator.SetTrigger("Attack");
        }

        Debug.Log("VAMPIRE ATTACK!");
    }

    public bool IsChasing()
    {
        return isChasing;
    }

    public void StopAtExit()
    {
        StartCoroutine(MoveToExitStopPoint());
    }

    private IEnumerator MoveToExitStopPoint()
    {
        isChasing = false;
        isAttacking = false;

        if (exitStopPoint == null)
            yield break;

        if (vampireAnimator != null)
        {
            vampireAnimator.SetFloat("Speed", 1f);
        }

        while (
            Vector2.Distance(transform.position, exitStopPoint.position) > 0.05f
        )
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                exitStopPoint.position,
                chaseSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = exitStopPoint.position;

        if (vampireAnimator != null)
        {
            vampireAnimator.SetFloat("Speed", 0f);
        }

        Debug.Log("Monster stopped at exit.");
    }

    private void FlipTowardsPlayer()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
}