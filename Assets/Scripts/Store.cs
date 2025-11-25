using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Store : MonoBehaviour
{
    [SerializeField] GameObject notification;
    [SerializeField] GameObject store;
    [SerializeField] GameObject player;

    [SerializeField] List<Image> abilityImages;
    [SerializeField] List<Sprite> abilitySprites;

    [SerializeField] List<Image> cardImages;

    [SerializeField] GameObject bleedObject1;
    [SerializeField] GameObject stunObject1;
    [SerializeField] GameObject slowObject1;
    [SerializeField] GameObject energyObject1;

    [SerializeField] GameObject bleedObject2;
    [SerializeField] GameObject stunObject2;
    [SerializeField] GameObject slowObject2;
    [SerializeField] GameObject energyObject2;

    [SerializeField] GameObject bleedObject3;
    [SerializeField] GameObject stunObject3;
    [SerializeField] GameObject slowObject3;
    [SerializeField] GameObject energyObject3;

    [SerializeField] GameObject bleedObject4;
    [SerializeField] GameObject stunObject4;
    [SerializeField] GameObject slowObject4;
    [SerializeField] GameObject energyObject4;

    [SerializeField] GameObject bleedObject5;
    [SerializeField] GameObject stunObject5;
    [SerializeField] GameObject slowObject5;
    [SerializeField] GameObject energyObject5;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && notification.activeSelf)
        {
            ActivateStore();
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
        }
    }

    public void Spin()
    {
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

    }

    public void SelectUpgrade()
    {

    }

}
