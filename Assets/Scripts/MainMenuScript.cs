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
        audioManagerScript.ChangeMusic(audioManagerScript.TownMusic);

        SceneManager.LoadScene("Level");
    }
}
