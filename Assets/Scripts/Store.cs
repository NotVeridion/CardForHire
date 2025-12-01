using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Store : MonoBehaviour
{
    [SerializeField] GameObject notification;
    [SerializeField] GameObject store;
    [SerializeField] GameObject player;

    [SerializeField] AudioManagerScript audioManagerScript;

    public int cardIndex;
    public int abilityIndex;

    public int CardIndex {  get { return cardIndex; } set { cardIndex = value; CheckUpgrade(); } }
    public int AbilityIndex {  get { return abilityIndex; } set {  abilityIndex = value; CheckUpgrade(); } }


    [SerializeField] TextMeshProUGUI abilityInfoText;

    [SerializeField] Button upgradeButton;

    List<Card> cards;
    [SerializeField] DeckManagerScript deckManager;

    //Spining Cards

    [SerializeField] GameObject buySpinGameObject;
    [SerializeField] GameObject selectUpgradeGameObject;

    [SerializeField] List<Image> abilityImages;
    [SerializeField] List<Sprite> abilitySprites;

    [SerializeField] List<Image> cardImages;

    [SerializeField] List<GameObject> bleedObjects;
    [SerializeField] List<GameObject> stunObjects;
    [SerializeField] List<GameObject> slowObjects;
    [SerializeField] List<GameObject> energyObjects;

    [SerializeField] PlayerScript playerInfo;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button buyButton;
    [SerializeField] int cost;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        deckManager = FindAnyObjectByType<DeckManagerScript>();
        audioManagerScript = FindAnyObjectByType<AudioManagerScript>();
        playerInfo = player.GetComponent<PlayerScript>();

        costText.text = "Cost " + cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && notification.activeSelf)
        {
            ActivateStore();
        }

        if(cost > playerInfo.playerCash)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
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
            buySpinGameObject.SetActive(true);
            selectUpgradeGameObject.SetActive(false);
        }
    }

    public void Spin()
    {
        audioManagerScript.PlayOneShotSFX(audioManagerScript.Button);

        List<Sprite> newAbilitySprites = new List<Sprite>(abilitySprites);

        Random.InitState(System.DateTime.Now.Millisecond);
        

        // Ability shuffle using Fisher-Yates algorithm
        for (int j = newAbilitySprites.Count - 1; j >= 0; j--)
        {
            int idx = Random.Range(j, newAbilitySprites.Count);
            Sprite temp = newAbilitySprites[idx];
            newAbilitySprites[idx] = newAbilitySprites[j];
            newAbilitySprites[j] = temp;
        }
        abilitySprites = newAbilitySprites;

        for(int i =0; i < abilityImages.Count; i++)
        {
            abilityImages[i].sprite = abilitySprites[i];
        }

        cards = deckManager.FillWorkingDeck();
        for(int i = 0; i < cardImages.Count; i++)
        {
            cardImages[i].sprite = cards[i].sprite;
            if (cards[i].bleed)
            {
                bleedObjects[i].SetActive(true);
            }
            else
            {
                bleedObjects[i].SetActive(false);
            }
            if (cards[i].slow)
            {
                slowObjects[i].SetActive(true);
            }
            else
            {
                slowObjects[i].SetActive(false);
            }
            if (cards[i].knockOut)
            {
                stunObjects[i].SetActive(true);
            }
            else
            {
                stunObjects[i].SetActive(false);
            }
            if (cards[i].energyRegain)
            {
                energyObjects[i].SetActive(true);
            }
            else
            {
                energyObjects[i].SetActive(false);
            }

        }
        CheckUpgrade();
    }

    public void CheckUpgrade()
    {
        switch (abilitySprites[abilityIndex].name)
        {
            case "Bleed":
                if (cards[cardIndex].bleed)
                {
                    upgradeButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                }
                break;

            case "Slow":
                if (cards[cardIndex].slow)
                {
                    upgradeButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                }
                break;

            case "Energy":
                if (cards[cardIndex].energyRegain)
                {
                    upgradeButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                }
                break;

            case "Stun":
                if (cards[cardIndex].knockOut)
                {
                    upgradeButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                }
                break;

            case "Add":
                upgradeButton.interactable = true;
                break;
        }
    }

    public void SelectUpgrade()
    {
        switch(abilitySprites[abilityIndex].name)
        {
            case "Bleed":
                cards[cardIndex].bleed = true;
                break;

            case "Slow":
                cards[cardIndex].slow = true;
                break;

            case "Energy":
                cards[cardIndex].energyRegain = true;
                break;

            case "Stun":
                cards[cardIndex].knockOut = true;
                break;

            case "Add":
                cards.Add(Instantiate(cards[cardIndex]));
                break;
        }
        deckManager.storedDeck = cards;
        for (int i = 0; i < cardImages.Count; i++)
        {
            cardImages[i].sprite = cards[i].sprite;
            if (cards[i].bleed)
            {
                bleedObjects[i].SetActive(true);
            }
            else
            {
                bleedObjects[i].SetActive(false);
            }
            if (cards[i].slow)
            {
                slowObjects[i].SetActive(true);
            }
            else
            {
                slowObjects[i].SetActive(false);
            }
            if (cards[i].knockOut)
            {
                stunObjects[i].SetActive(true);
            }
            else
            {
                stunObjects[i].SetActive(false);
            }
            if (cards[i].energyRegain)
            {
                energyObjects[i].SetActive(true);
            }
            else
            {
                energyObjects[i].SetActive(false);
            }

        }
    }


    public void AbilityTextInfo(int i)
    {
        switch (abilitySprites[i].name)
        {
            case "Bleed":
                abilityInfoText.text = "Applies a bleed to enemies.";
                break;

            case "Slow":
                abilityInfoText.text = "Applies a slow to enemies.";
                break;

            case "Energy":
                abilityInfoText.text = "Reduce the cooldown of dash.";
                break;

            case "Stun":
                abilityInfoText.text = "A chance to stun enemies.";
                break;

            case "Add":
                abilityInfoText.text = "Create a copy of selected card.";
                break;
        }
    }

    public void ClearTextInfo()
    {
        abilityInfoText.text = string.Empty;
    }

}
