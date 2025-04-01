using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float enLeft;
    public float misLeft;

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

    private void Update()
    {
        if (gameState == GameStates.Playing)
        {
            playerTime += Time.deltaTime;
            if (bombPlanted)
            {
                bombTimer -= Time.deltaTime;
                if (bombTimer <= 0)
                {
                    GameOver();
                }
            }


            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void EnemyExit()
    {
        enemiesEscaped++;
        enemyEscapePoints += 200;
        totalPoints -= 200;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            gameState = GameStates.Paused;
            Time.timeScale = 0;
        }
        else
        {
            gameState = GameStates.Playing;
            Time.timeScale = 1;
        }
    }
    
    public void LevelComplete()
    {
        misLeft = Player.Instance.rocketAmount;
        enLeft = Player.Instance.energyAmount;
        MusicManager.Instance.PlayMenuMusic();
        GameManager.Instance.gameState = GameStates.GameOver;
        SceneManager.LoadScene("GameVictory");
    }
    public void GameOver()
    {
        GameManager.Instance.gameState = GameStates.GameOver;
        MusicManager.Instance.PlayMenuMusic();
        SceneManager.LoadScene("GameOver");
    }
    public void SetStart()
    {
        totalPoints = 0;
        enemyKillPoints = 0;
        enemyEscapePoints = 0;
        soldierSavedPoints = 0;
        soldierKilledPoints = 0;
        hitless = true;
        playerTime = 0;
        totalEnemies = 0;
        enemiesEscaped = 0;
        enemiesKilled = 0;
        bombPlanted = false;
        bombTimer = 180;
        playerHealth = 100;
        gameState = GameStates.Playing;
    }
}
