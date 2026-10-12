using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDistance = 1.3f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private float lungeForce = 4.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyMovement movement;
    private float lastAttackTime;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
    }

    private PlayerMovement playerMovement;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerMovement = p.GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
        if (player == null || isAttacking) return;

        if (playerMovement != null && !playerMovement.enabled) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        if (movement != null) movement.enabled = false;

        if (animator != null)
        {
            animator.SetInteger("AnimState", 0);
            animator.SetTrigger("Attack1");
        }

        Vector2 attackDir = (player.position - transform.position).normalized;
        rb.linearVelocity = attackDir * lungeForce;

        yield return new WaitForSeconds(0.25f);

        rb.linearVelocity = Vector2.zero;

        if (player != null && Vector2.Distance(transform.position, player.position) <= attackDistance + 0.6f)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(0.4f);

        if (movement != null) movement.enabled = true;
        isAttacking = false;
    }
}