using System;
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

    public float totalPoints = 0f;
    public float enemyKillPoints = 0f;
    public float enemyEscapePoints = 0f;
    public float soldierSavedPoints = 0f;
    public float soldierKilledPoints = 0f;
    public bool hitless = true;
    public float playerTime = 0f;

    public int totalEnemies = 0;
    public int enemiesEscaped = 0;
    public int enemiesKilled = 0;
    public bool bombPlanted = false;
    public float bombTimer = 180f;

    public bool controllerMode = false;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnemyExit()
    {
        enemiesEscaped++;
        enemyEscapePoints -= 200;
        totalPoints -= 200;
    }

    public void LevelComplete()
    {
        MusicManager.Instance.PlayMenuMusic();
        Debug.Log("Level Complete");
    }
    public void GameOver()
    {
        MusicManager.Instance.PlayMenuMusic();
        Debug.Log("Game over");
    }
}
