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
        Vector3 newDirection = Player.Instance.transform.position - transform.position;
        direction = (direction + newDirection) / 2;
    }
}
