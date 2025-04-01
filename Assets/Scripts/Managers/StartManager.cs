using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMenuMusic();
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        { SceneManager.LoadScene("MainMenu"); }
    }
}
