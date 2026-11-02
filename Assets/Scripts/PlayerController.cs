using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How fast the player moves, in units per second.")]
    public float moveSpeed = 3.5f;

    [Header("References")]
    private Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down; // used to face the right way while idle

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Recommended settings for a top-down character:
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }

        //UpdateAnimator();
    }

    void FixedUpdate()
    {
        // Move using physics so collision with walls/objects works correctly
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking",true);
        if(context.canceled)
        {
            animator.SetBool("isWalking",false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        moveInput = context.ReadValue<Vector2>().normalized;

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        bool isMoving = moveInput != Vector2.zero;

        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);

        // In your Animator, set up 4 blend states (or a Blend Tree) using
        // MoveX / MoveY to pick which direction the walk cycle faces:
        //   Down:  MoveY < 0
        //   Up:    MoveY > 0
        //   Left:  MoveX < 0
        //   Right: MoveX > 0
    }
}