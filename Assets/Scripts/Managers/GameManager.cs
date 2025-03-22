using UnityEngine;

public enum GameStates
{
    Menu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public GameStates gameState = GameStates.Menu;

    public float playerHealth = 100;
    public float laserAmount = 100;
    public float rocketAmount = 100;

    public float totalPoints = 0f;
    public float playerTime = 0f;
    public int totalEnemies = 0;
    public int enemiesEscaped = 0;

    public bool controllerMode = false;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
