using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public float health { get; set; }

    public float slimeExplosionRadius = 1;
    public float slimeDamage = 4;

    private void Start()
    {
        GameManager.Instance.totalEnemies++;
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
