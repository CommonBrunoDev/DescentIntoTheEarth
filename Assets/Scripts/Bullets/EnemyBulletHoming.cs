using UnityEngine;

public class EnemyBulletHoming : Bullet
{
    [SerializeField][Range(0,1)] float rotationPower = 1f;
    [SerializeField] float damage = 1f;
    private new void Update()
    {
        RotateBullet();
        base.Update();
    }

    void RotateBullet()
    {
        float power = 2 - rotationPower;
        Vector3 newDirection = (Player.Instance.transform.position - transform.position).normalized;
        direction = (direction * power + newDirection * rotationPower) / 2;
        transform.rotation = Quaternion.LookRotation(direction);
    }
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
