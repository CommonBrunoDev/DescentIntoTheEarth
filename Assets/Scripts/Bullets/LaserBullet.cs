using UnityEngine;

public class LaserBullet : Bullet
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
                damageable.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
