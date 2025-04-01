using UnityEngine;

public class BombArea : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool playerInArea = false;


    private void Start()
    {
        spriteRenderer.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (playerInArea && !GameManager.Instance.bombPlanted)
        {
            Player.Instance.bombPopup.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 2"))
            {
                GameManager.Instance.bombPlanted = true;
                Player.Instance.HandleActivation();
                spriteRenderer.gameObject.SetActive(true);
                MusicManager.Instance.PlayAlertMusic();
            }
        }
        else
        {
            Player.Instance.bombPopup.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.bombPlanted)
        { playerInArea = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.bombPlanted)
        { playerInArea = false; }
    }
}
