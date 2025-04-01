using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float respawnTime = 5f;
    private float respawnTimer = 5f;
    public bool respawning = false;
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Player.Instance.transform.position - transform.position);

        if (respawning)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                respawning = false;
                respawnTimer = respawnTime;
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!respawning)
        {
            BodyEntered(other); 
        }
    }

    protected virtual void BodyEntered(Collider other)
    {
        Debug.Log("Base pickup entered!");
    }
}
