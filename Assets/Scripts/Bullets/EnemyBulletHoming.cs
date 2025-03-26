using UnityEngine;

public class EnemyBulletHoming : EnemyBulletNormal
{
    private new void Update()
    {
        RotateBullet();
        base.Update();
    }

    void RotateBullet()
    {
        Vector3 newDirection = (Player.Instance.transform.position - transform.position).normalized;
        direction = (direction * 1.85f + newDirection * 0.15f) / 2;
    }
}
