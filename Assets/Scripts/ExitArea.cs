using UnityEngine;

public class ExitArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.bombPlanted)
            {
                GameManager.Instance.LevelComplete();
            }
            else
            {
                Player.Instance.exitPopup.SetActive(true);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.EnemyExit();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.exitPopup.SetActive(false);
        }
    }
}
