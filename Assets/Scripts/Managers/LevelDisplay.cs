using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
