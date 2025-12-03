using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUISccript : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] AudioManagerScript audioManagerScript;
    [SerializeField] BossScript bossScript;

    [SerializeField] Slider health;
    [SerializeField] Slider dashCoolDown;
    [SerializeField] Image dashSliderImage;

    [SerializeField] GameObject dashDistanceAbility;
    [SerializeField] GameObject attackSpeedAbility;
    [SerializeField] GameObject damageAbility;
    [SerializeField] GameObject movementSpeedAbility;

    [SerializeField] Image dashDistanceSlider;
    [SerializeField] Image attackSpeedSlider;
    [SerializeField] Image damageSlider;
    [SerializeField] Image movementSpeedSlider;


    [SerializeField] Image cardImage;

    [SerializeField] DeckManagerScript deckManager;
    [SerializeField] GameObject bleedObject;
    [SerializeField] GameObject stunObject;
    [SerializeField] GameObject slowObject;
    [SerializeField] GameObject energyObject;

    [SerializeField] TextMeshProUGUI moneyText;

    //Pausing
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject storeUI;
    [SerializeField] GameObject gameoverPanel;
    [SerializeField] GameObject storeUI2;

    [SerializeField] GameObject respawnPosition;

    //Settings
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Settings settings;


    //Deck Selection
    [SerializeField] Deck standard;
    [SerializeField] Deck red;
    [SerializeField] Deck black;

    float currentDamage;
    float startDamage;
    float currentAS;
    float startAS;
    float currentMove;
    float startMove;
    float currentDash;
    float startDash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deckManager = FindAnyObjectByType<DeckManagerScript>();
        playerScript = FindAnyObjectByType<PlayerScript>();
        audioManagerScript = FindAnyObjectByType<AudioManagerScript>();
        bossScript = FindAnyObjectByType<BossScript>();

        playerScript.gameObject.SetActive(false);

        musicSlider.value = settings.musicSliderValue;
        SFXSlider.value = settings.SFXSliderValue;
    }

    // Update is called once per frame
    void Update()
    {

        // Update settings
        settings.musicSliderValue = musicSlider.value;
        settings.SFXSliderValue = SFXSlider.value;

        cardImage.sprite = deckManager.getCurrentCard().sprite;
        if(deckManager.getCurrentCard().bleed)
        {
            bleedObject.SetActive(true);
        }
        else
        {
            bleedObject.SetActive(false);
        }
        if(deckManager.getCurrentCard().slow)
        {
            slowObject.SetActive(true);
        }
        else
        {
            slowObject.SetActive(false);
        }
        if(deckManager.getCurrentCard().energyRegain)
        {
            energyObject.SetActive(true);
        }
        else
        {
            energyObject.SetActive(false);
        }
        if(deckManager.getCurrentCard().knockOut)
        {
            stunObject.SetActive(true);
        }
        else
        {
            stunObject.SetActive(false);
        }

        health.value = playerScript.playerHealth / 100f;
        dashCoolDown.value =1 - playerScript.currentDashCooldown / playerScript.dashCooldown;
        if(dashCoolDown.value >= 1)
        {
            dashSliderImage.color = Color.green;
        }
        else
        {  
            dashSliderImage.color = Color.gray;
        }

        moneyText.text = playerScript.playerCash.ToString();

        dashDistanceSlider.fillAmount = 1;

        if (currentDash >0)
        {
            dashDistanceSlider.fillAmount = currentDash / startDash;
            currentDash -= Time.deltaTime;
        }
        else
        {
            dashDistanceAbility.SetActive(false);
        }

        if(currentAS > 0)
        {
            attackSpeedSlider.fillAmount = currentAS / startAS;
            currentAS -= Time.deltaTime;
        }
        else
        {
            attackSpeedAbility.SetActive(false);
        }

        if(currentMove > 0)
        {
            movementSpeedSlider.fillAmount = currentMove / startMove;
            currentMove -= Time.deltaTime;
        }
        else
        {
            movementSpeedAbility.SetActive(false);
        }

        if(currentDamage > 0)
        {
            damageSlider.fillAmount = currentDamage / startDamage;
            currentDamage -= Time.deltaTime;
        }
        else
        {
            damageAbility.SetActive(false);
        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if(playerScript.playerHealth <= 0)
        {
            GameOver();
        }
    }

    public void Pause()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);

        if (gameoverPanel.activeSelf)
        {
            return;
        }

        if (pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);

            // Avoid reenabling the player if they were previously in a store before pausing
            // Fixes bug
            if (storeUI.activeSelf || storeUI2.activeSelf)
            {
                playerScript.gameObject.SetActive(false);
            }
            else
            {
                playerScript.gameObject.SetActive(true);
                GameObject.FindWithTag("Gun").GetComponent<GunScript>().RestoreFire();
            }

        }
        else
        {
            if (settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(false);
            }

            pausePanel.SetActive(true);
            playerScript.gameObject.SetActive(false);
        }
    }

    public void Settings()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        settingsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void SettingsBack()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Quit()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        Application.Quit();
    }

    public void SelectDeck(string deckName)
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        if(deckName == "Red")
        {
            deckManager.chosenDeck = red;
        }
        else if(deckName == "Black")
        {
            deckManager.chosenDeck = black;
        }
        else
        {
            deckManager.chosenDeck = standard;
        }
        deckManager.StartDeck();
        playerScript.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gameoverPanel.SetActive(true);
    }

    public void MainMenu()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        SceneManager.LoadScene("MainMenu");
    }

    public void RespawnPlayer()
    {
        playerScript.gameObject.transform.position = respawnPosition.transform.position;

        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        audioManagerScript.ChangeMusic(audioManagerScript.TownMusic);
        bossScript.StateMachine.ChangeState(bossScript.IdleState);
        Camera.main.GetComponent<CameraScript>().inFinalBoss = false;

        playerScript.location = "Sheriff";
        playerScript.Heal(1000);
        playerScript.isMovementLocked = false;

        gameoverPanel.SetActive(false);
        GameObject.FindWithTag("Gun").GetComponent<GunScript>().RestoreFire();
    }
    
    public void AttackSpeedDuration(float duration)
    {
        startAS = duration;
        currentAS = startAS;
        attackSpeedAbility.SetActive(true);
    }

    public void DamageDuration(float duration)
    {
        startDamage = duration;
        currentDamage = startDamage;
        damageAbility.SetActive(true);
    }


    public void MovementDuration(float duration)
    {
        startMove = duration;
        currentMove = duration;
        movementSpeedAbility.SetActive(true);
    }

    public void DashDistance(float duration)
    {
        startDash = duration;
        currentDash = duration;
        dashDistanceAbility.SetActive(true);
    }
}
