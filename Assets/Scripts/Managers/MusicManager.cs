using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip gameAudio;
    [SerializeField] AudioClip menuAudio;

    [SerializeField] AudioSource mainPlayer;
    [SerializeField] AudioSource alertPlayer;

    private static MusicManager instance;
    public static MusicManager Instance { get { return instance; } }

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

    private void Start()
    {
        PlayGameMusic();
    }

    public void PlayGameMusic()
    {
        mainPlayer.clip = gameAudio;
        mainPlayer.Play(0);
    }
    
    public void PlayAlertMusic()
    {
        alertPlayer.Play(0);
    }

    public void PlayMenuMusic()
    {
        if (alertPlayer.isPlaying)
            alertPlayer.Stop();

        mainPlayer.clip = menuAudio;
        mainPlayer.Play(0);
    }

}
