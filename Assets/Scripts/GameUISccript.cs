using UnityEngine;
using UnityEngine.UI;

public class GameUISccript : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;

    [SerializeField] Slider health;
    [SerializeField] Slider dashCoolDown;

    [SerializeField] GameObject dashDistanceAbility;
    [SerializeField] GameObject attackSpeedAbility;
    [SerializeField] GameObject damageAbility;
    [SerializeField] GameObject movementSpeedAbility;

    [SerializeField] Image cardImage;

    [SerializeField] DeckManagerScript deckManager;
    [SerializeField] GameObject bleedObject;
    [SerializeField] GameObject stunObject;
    [SerializeField] GameObject slowObject;
    [SerializeField] GameObject energyObject;

    //Pausing
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject storeUI;
    [SerializeField] GameObject gameoverPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deckManager = FindAnyObjectByType<DeckManagerScript>();
        playerScript = FindAnyObjectByType<PlayerScript>();
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
        dashCoolDown.value = playerScript.currentDashCooldown / playerScript.dashCooldown;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    public void Pause()
    {
        if (pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            playerScript.gameObject.SetActive(true);
            storeUI.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(true);
            playerScript.gameObject.SetActive(false);
            storeUI.SetActive(false );  
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}
