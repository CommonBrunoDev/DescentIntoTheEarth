using UnityEngine;

public class EnemyBulletHoming : EnemyBulletNormal
{
    [SerializeField][Range(0,1)] float rotationPower = 1f;
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
    }
}
