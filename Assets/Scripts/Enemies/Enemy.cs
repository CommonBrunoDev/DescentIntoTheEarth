using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public float health { get; set; }

    public float slimeExplosionRadius = 1;
    public float slimeDamage = 4;
    public float pointsAwarded = 0;

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.totalEnemies++;

        health = 100;
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
