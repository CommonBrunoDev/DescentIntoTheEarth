using UnityEngine;

public class MissilePickup : Pickup
{
    public float missileReload = 5f;
    protected override void BodyEntered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.rocketAmount += missileReload;
        }
    }
}
