using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndScript : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
