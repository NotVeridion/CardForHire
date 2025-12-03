using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndScript : MonoBehaviour
{
    private AudioManagerScript audioManagerScript;

    public void Start()
    {
        audioManagerScript = FindAnyObjectByType<AudioManagerScript>();
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        audioManagerScript.ChangeMusic(audioManagerScript.MainMenuMusic);
        SceneManager.LoadScene("MainMenu");
    }
}
