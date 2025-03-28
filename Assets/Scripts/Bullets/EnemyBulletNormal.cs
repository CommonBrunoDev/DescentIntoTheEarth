using UnityEngine;

public class EnemyBulletNormal : Bullet
{
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider collision)
    {
        transform.rotation = Quaternion.LookRotation(direction);

        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject != parent)
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
