using UnityEngine;

public class LaserPickup : Pickup
{
    public float energySet = 300;
    protected override void BodyEntered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.energyAmount = energySet;
            GetComponent<SpriteRenderer>().enabled = false;
            respawning = true;
        }
    }
}
