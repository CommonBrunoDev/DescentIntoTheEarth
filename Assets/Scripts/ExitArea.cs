using UnityEngine;

public class ExitArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player");
            if (GameManager.Instance.bombPlanted)
            {
                GameManager.Instance.LevelComplete();
            }
            else
            {
                Player.Instance.cantEscape.SetActive(true);
                Player.Instance.escapeVisTimer = Player.Instance.escapeVisTime;
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy");
            GameManager.Instance.EnemyExit();
            Player.Instance.enemyEscaped.SetActive(true);
            Player.Instance.enemyVisTimer = Player.Instance.enemyVisTime;
            Destroy(other.gameObject);
        }
    }
}
