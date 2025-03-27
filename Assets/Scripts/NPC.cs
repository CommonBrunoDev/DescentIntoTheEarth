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
}
