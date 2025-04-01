using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralSceneManager : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown("joystick button 6") || Input.GetKeyDown("joystick button 7"))
        { 
            var scene = SceneManager.GetActiveScene();
            if (scene == SceneManager.GetSceneByName("MainMenu"))
            { NewGame(); }
            else if (scene == SceneManager.GetSceneByName("GameVictory") || scene == SceneManager.GetSceneByName("GameOver"))
            { SceneMain(); }
        }
    }
    public void SceneMain()
    { SceneManager.LoadScene("MainMenu"); }

    public void SceneIncomplete()
    { SceneManager.LoadScene("Incomplete"); }

    public void SceneCommandsKeyboard()
    { SceneManager.LoadScene("CommandsKey"); }

    public void SceneCommandsJoycon()
    { SceneManager.LoadScene("CommandsJoy"); }

    public void SceneCredits()
    { SceneManager.LoadScene("Credits"); }

    public void SceneConfirm()
    { SceneManager.LoadScene("Confirm"); }
    public void SceneLevels()
    { SceneManager.LoadScene("Levels"); }
    public void NewGame()
    {
        GameManager.Instance.SetStart();
        MusicManager.Instance.PlayGameMusic();
        GameManager.Instance.gameState = GameStates.Playing;
        SceneManager.LoadScene("test");
    }
    public void CloseGame()
    { Application.Quit(); }
}
