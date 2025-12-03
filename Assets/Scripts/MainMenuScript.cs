using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private AudioManagerScript audioManagerScript;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    
    [SerializeField] Settings settings;

    void Start()
    {
        musicSlider.value = settings.musicSliderValue;
        SFXSlider.value = settings.SFXSliderValue;
    }

    void Update()
    {
        audioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManagerScript>();

        // Update settings
        settings.musicSliderValue = musicSlider.value;
        settings.SFXSliderValue = SFXSlider.value;
    }
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
