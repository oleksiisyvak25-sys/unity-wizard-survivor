using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnDeath;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();

        if (CompareTag("Player"))
        {
            StartCoroutine(PlayerDeathRoutine());
        }
        else
        {
            GameManager.Instance?.AddScore(1);

            StartCoroutine(EnemyDeathRoutine());
        }
    }

    private System.Collections.IEnumerator PlayerDeathRoutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        PlayerMovement move = GetComponent<PlayerMovement>();
        if (move != null) move.enabled = false;

        AutoWeapon weapon = GetComponent<AutoWeapon>();
        if (weapon != null) weapon.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Death");

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null) spawner.enabled = false;

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance?.GameOver();
    }

    private IEnumerator EnemyDeathRoutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move != null) move.enabled = false;

        EnemyAttack atk = GetComponent<EnemyAttack>();
        if (atk != null) atk.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Death");

        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}