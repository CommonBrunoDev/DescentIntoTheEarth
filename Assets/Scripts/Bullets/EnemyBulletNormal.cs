using UnityEngine;

public class EnemyBulletNormal : Bullet
{
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject != parent)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (collision.CompareTag("Player"))
                { collision.GetComponent<Player>().Slimed(); }

                damageable.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
