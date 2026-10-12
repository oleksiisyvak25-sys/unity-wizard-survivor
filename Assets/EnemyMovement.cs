using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;

    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy || (playerMovement != null && !playerMovement.enabled))
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetInteger("AnimState", 0);
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0.05f)
            spriteRenderer.flipX = false;
        else if (direction.x < -0.05f)
            spriteRenderer.flipX = true;

        if (animator != null)
        {
            animator.SetInteger("AnimState", 1);
        }
    }
}