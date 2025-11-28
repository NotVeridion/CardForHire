using UnityEngine;
using System.Collections.Generic;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] GameObject teleportPosition;
    [SerializeField] GameObject player;
    [SerializeField] GameObject teleportInfo;

    [SerializeField] bool dangerous;
    [SerializeField] bool spawn;
    [SerializeField] List<GameObject> spawnPositions;
    [SerializeField] List<GameObject> enemiesPrefab;
    public List<GameObject> currentEnemies;
    public GameObject spawnHolder;

    private AudioManagerScript audioManagerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (spawn)
        {
            for (int i = 0; i < spawnHolder.transform.childCount; i++)
            {
                spawnPositions.Add(spawnHolder.transform.GetChild(i).gameObject);
            }
        }

        audioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManagerScript>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && teleportInfo.activeSelf)
        {
            Teleport();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        teleportInfo.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        teleportInfo.SetActive(false);
    }

    void Teleport()
    {
        player.transform.position = teleportPosition.transform.position;
        teleportInfo.SetActive(false );
        if(dangerous)
        {
            if (spawn)
            {
                SpawnEnemies();
            }
            else
            {
                DeSpawnEnemies();
            }
        }

        ChangeMusic();
    }

    void ChangeMusic()
    {
        AudioClip musicClip = null;
        if (teleportPosition.CompareTag("TownLocation"))
        {
            player.GetComponent<PlayerScript>().location = "Town";
            musicClip = audioManagerScript.TownMusic;
        }
        else if (teleportPosition.CompareTag("ShopLocation"))
        {
            player.GetComponent<PlayerScript>().location = "Shop";
            musicClip = audioManagerScript.ShopMusic;
        }
        // Going into houses will keep the town music
        else if (teleportPosition.CompareTag("SheriffLocation"))
        {
            player.GetComponent<PlayerScript>().location = "Sheriff";
        }
        else if (teleportPosition.CompareTag("NPCHouseLocation"))
        {
            player.GetComponent<PlayerScript>().location = "NPCHouse";
        }
        //
        else if (teleportPosition.CompareTag("CasinoLocation"))
        {
            player.GetComponent<PlayerScript>().location = "Casino";
            musicClip = audioManagerScript.DungeonMusic;
        }
        else if (teleportPosition.CompareTag("CaveLocation"))
        {
            player.GetComponent<PlayerScript>().location = "Cave";
            musicClip = audioManagerScript.CaveMusic;
        }

        audioManagerScript.ChangeMusic(musicClip);
    }

    void SpawnEnemies()
    {
        if(currentEnemies.Count > 0)
        {
            foreach(GameObject enemy in currentEnemies)
            {
                Destroy(enemy);
            }
            currentEnemies.Clear();
        }
        foreach(GameObject point in spawnPositions)
        {
            currentEnemies.Add(Instantiate(enemiesPrefab[Random.Range(0, enemiesPrefab.Count)], point.transform));
        }
    }

    void DeSpawnEnemies()
    {
        currentEnemies = teleportPosition.GetComponent<TeleportScript>().currentEnemies;
        foreach (GameObject enemy in currentEnemies)
        {
            Destroy(enemy);
        }
        currentEnemies.Clear();
        teleportPosition.GetComponent<TeleportScript>().currentEnemies.Clear();
    }

}
