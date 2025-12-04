using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store2 : MonoBehaviour
{
    [SerializeField] GameObject notification;
    [SerializeField] GameObject store;
    [SerializeField] GameObject player;
    [SerializeField] PlayerScript playerInfo;

    [SerializeField] GunScript gun;

    [SerializeField] TextMeshProUGUI fireRateCostText;
    [SerializeField] TextMeshProUGUI damageCostText;
    [SerializeField] TextMeshProUGUI bulletCountCostText;

    [SerializeField] Button fireRateButton;
    [SerializeField] Button damageButton;
    [SerializeField] Button bulletCountButton;

    [SerializeField] int fireRateCost;
    [SerializeField] int damageCost;
    [SerializeField] int bulletCountCost;

    [SerializeField] float damageIncrease;
    [SerializeField] float fireRateIncrease;
    [SerializeField] float rangeIncrease;

    [SerializeField] AudioManagerScript audioManagerScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gun = FindAnyObjectByType<GunScript>();
        audioManagerScript = FindAnyObjectByType<AudioManagerScript>();
        playerInfo = player.GetComponent<PlayerScript>();

        fireRateCostText.text = "Cost " + fireRateCost.ToString();
        damageCostText.text = "Cost "+ damageCost.ToString();
        bulletCountCostText.text = "Cost " + bulletCountCost.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && notification.activeSelf)
        {
            ActivateStore();
        }

        if(fireRateCost > playerInfo.playerCash)
        {
            fireRateButton.interactable = false;
        }
        else
        {
            fireRateButton.interactable = true;
        }
        if (damageCost > playerInfo.playerCash)
        {
            damageButton.interactable = false;
        }
        else
        {
            damageButton.interactable = true;
        }
        if (bulletCountCost > playerInfo.playerCash)
        {
            bulletCountButton.interactable = false;
        }
        else
        {
            bulletCountButton.interactable = true;
        }


    }

    public void BuyDamage()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        gun.currentGun.damage += damageIncrease;
        playerInfo.playerCash -= damageCost;
    }

    public void BuyFireRate()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        gun.currentGun.fireRate += fireRateIncrease;
        playerInfo.playerCash -= fireRateCost;
    }

    public void BuyBulletCount()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);
        if (gun.currentGun.isSingleShot)
        {
            gun.currentGun.isSingleShot = false;
            gun.currentGun.isSpreadShot = true;
            gun.currentGun.numBulletsInSpread = 1;
        }

        gun.currentGun.numBulletsInSpread += 1;
        gun.currentGun.spreadRange += rangeIncrease;

        playerInfo.playerCash -= bulletCountCost;
    }


    public void ActivateStore()
    {
        if (notification.activeSelf)
        {
            store.SetActive(true);
            player.SetActive(false);
        }
        else
        {
            notification.SetActive(true);
            store.SetActive(false);
            player.SetActive(true);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        notification.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        notification.SetActive(false);
    }
}
