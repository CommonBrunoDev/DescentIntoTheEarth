using UnityEngine;

public class NPC : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Player.Instance.transform.position - transform.position);
    }

    private void OnTriggerEnter(Collider other)
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
        Debug.Log("NPC saved!");
    }
    private void KillNPC()
    {
        Debug.Log("NPC killed!");
    }
}
