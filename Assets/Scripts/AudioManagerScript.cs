using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerScript : MonoBehaviour
{
    [Header("   Audio Sources   ")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("   Location Music  ")]
    public AudioClip MainMenuMusic;
    public AudioClip TownMusic;
    public AudioClip CaveMusic;
    public AudioClip ShopMusic;
    public AudioClip DungeonMusic;
    public AudioClip BossMusic;
    public AudioClip EndMusic;

    [Header("   Sound Effects  ")]
    public AudioClip Pickup;
    public AudioClip Hit;
    public AudioClip Dash;
    public AudioClip Button;
    public AudioClip[] ThroughDoor;
    public AudioClip[] WalkingGrass;
    public AudioClip[] WalkingStone;
    public AudioClip[] Shooting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] audioObjs = GameObject.FindGameObjectsWithTag("AudioManager");
        if (audioObjs.Length > 1)
        {
            foreach (GameObject obj in audioObjs){
                if (obj != gameObject)
                {
                    Destroy(obj);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
        musicSource.clip = MainMenuMusic;
        musicSource.Play();
    }

    public void PlayOneShotSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    
    public void PlayRandomSFX(AudioClip[] clips)
    {
        SFXSource.PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
    }

    public void ChangeMusic(AudioClip music)
    {
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
    }
}
