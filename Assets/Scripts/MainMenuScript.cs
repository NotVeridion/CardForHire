using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AudioManagerScript audioManagerScript;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        audioManagerScript.musicSource.Stop();
        
        audioManagerScript.musicSource.clip = audioManagerScript.TownMusic;
        audioManagerScript.musicSource.Play();

        SceneManager.LoadScene("Level");
    }
}
