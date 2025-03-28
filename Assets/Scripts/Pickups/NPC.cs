using UnityEngine;

public class NPC : Pickup
{
    protected override void BodyEntered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveNPC();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            KillNPC();
            Destroy(gameObject);
        }
    }
    private void SaveNPC()
    {
        GameManager.Instance.soldierSavedPoints += 200f;
    }
    private void KillNPC()
    {
        GameManager.Instance.soldierKilledPoints += 200f;
    }
}
