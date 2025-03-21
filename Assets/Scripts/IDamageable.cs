using UnityEngine;

public interface IDamageable
{
    float health { get; }
    void TakeDamage(float damage);
    void Destroy();
}
