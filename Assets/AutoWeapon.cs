using UnityEngine;

public class AutoWeapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private float attackRange = 9f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                ShootAt(nearestEnemy.position);
                timer = 0f;
            }
        }
    }

    private Transform FindNearestEnemy()
    {
        Health[] allUnits = FindObjectsByType<Health>(FindObjectsSortMode.None);
        Transform nearest = null;
        float minDistance = attackRange;

        foreach (Health unit in allUnits)
        {
            if (unit.CompareTag("Player")) continue;

            float dist = Vector2.Distance(transform.position, unit.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = unit.transform;
            }
        }

        return nearest;
    }

    private void ShootAt(Vector3 targetPos)
    {
        if (projectilePrefab == null) return;

        Vector2 direction = (targetPos - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.Setup(direction);
        }
    }
}