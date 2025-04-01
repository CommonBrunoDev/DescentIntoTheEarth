using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    public float health { get; set; }

    public float startingHealth = 5;
    public float detectionDistance = 0;

    public float slimeExplosionRadius = 1;
    public float slimeDamage = 4;
    public float pointsAwarded = 0;

    protected void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.totalEnemies++;

        health = startingHealth;
    }

    public void TakeDamage(float damage)
    {
        if (health - damage <= 0)
        { Death(); }
        else
        { health -= damage; }
    }
    public void Death()
    {
        Slimify();
        GameManager.Instance.totalPoints += pointsAwarded;
        GameManager.Instance.enemyKillPoints += pointsAwarded;
        GameManager.Instance.enemiesKilled++;
        Destroy(gameObject);
    }
    public void Slimify()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, slimeExplosionRadius);

        foreach (Collider hit in colliders)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && hit.CompareTag("Player"))
            {
                damageable.TakeDamage(slimeDamage);
                hit.GetComponent<Player>().Slimed();
            }
        }
    }
}
