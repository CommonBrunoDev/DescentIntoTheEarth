using UnityEngine;

public class HealthPickup : Pickup
{
    public float healPower = 100;
    protected override void BodyEntered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.HP = Mathf.Clamp(Player.Instance.health + healPower,0,100);
            Destroy(gameObject);
        }
    }
}
