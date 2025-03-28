using UnityEngine;

public class BombArea : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool playerInArea = false;
    public bool activated = false;


    private void Start()
    {
        spriteRenderer.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (playerInArea && !GameManager.Instance.bombPlanted)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 2"))
            {
                GameManager.Instance.bombPlanted = true;
                Player.Instance.HandleActivation();
                Player.Instance.bombPopup.SetActive(false);
                MusicManager.Instance.PlayAlertMusic();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.bombPlanted)
        {
            Player.Instance.bombPopup.SetActive(true);
            playerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.bombPlanted)
        {
            Player.Instance.bombPopup.SetActive(false);
            playerInArea = false;
        }
    }
}
