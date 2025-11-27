using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUISccript : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;

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


    //Deck Selection
    [SerializeField] Deck standard;
    [SerializeField] Deck red;
    [SerializeField] Deck black;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deckManager = FindAnyObjectByType<DeckManagerScript>();
        playerScript = FindAnyObjectByType<PlayerScript>();

        playerScript.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
        if(deckManager.getCurrentCard().stun)
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
        if (gameoverPanel.activeSelf)
        {
            return;
        }

        if (pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            playerScript.gameObject.SetActive(true);
            storeUI.SetActive(true);
            storeUI2.SetActive(true);
            GameObject.FindWithTag("Gun").GetComponent<GunScript>().RestoreFire();
        }
        else
        {
            pausePanel.SetActive(true);
            playerScript.gameObject.SetActive(false);
            storeUI.SetActive(false );
            storeUI2.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SelectDeck(string deckName)
    {
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
        playerScript.gameObject.SetActive(false);
        gameoverPanel.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RespawnPlayer()
    {
        playerScript.gameObject.transform.position = respawnPosition.transform.position;
        playerScript.Heal(1000);
        playerScript.gameObject.SetActive(true);
        gameoverPanel.SetActive(false);
        GameObject.FindWithTag("Gun").GetComponent<GunScript>().RestoreFire();

    }

}
