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
        teleportInfo.SetActive(false);

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
        if (collision.gameObject.CompareTag("Player"))
        {
            teleportInfo.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            teleportInfo.SetActive(false);
        }
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
            if (player.GetComponent<PlayerScript>().location == "Sheriff" || player.GetComponent<PlayerScript>().location == "NPCHouse")
            {
                player.GetComponent<PlayerScript>().location = "Town";
                return;
            }

            player.GetComponent<PlayerScript>().location = "Town";
            musicClip = audioManagerScript.TownMusic;
            audioManagerScript.ChangeMusic(musicClip);
            audioManagerScript.PlayRandomSFX(audioManagerScript.ThroughDoor);
        }
        else if (teleportPosition.CompareTag("ShopLocation"))
        {
            musicClip = audioManagerScript.ShopMusic;
            audioManagerScript.ChangeMusic(musicClip);
            audioManagerScript.PlayRandomSFX(audioManagerScript.ThroughDoor);
            player.GetComponent<PlayerScript>().location = "Shop";
        }
        // Going into houses will keep the town music
        else if (teleportPosition.CompareTag("SheriffLocation"))
        {
            audioManagerScript.PlayRandomSFX(audioManagerScript.ThroughDoor);
            player.GetComponent<PlayerScript>().location = "Sheriff";
        }
        else if (teleportPosition.CompareTag("NPCHouseLocation"))
        {
            audioManagerScript.PlayRandomSFX(audioManagerScript.ThroughDoor);
            player.GetComponent<PlayerScript>().location = "NPCHouse";
        }
        //
        else if (teleportPosition.CompareTag("CasinoLocation"))
        {
            musicClip = audioManagerScript.DungeonMusic;
            audioManagerScript.ChangeMusic(musicClip);
            player.GetComponent<PlayerScript>().location = "Casino";
        }
        else if (teleportPosition.CompareTag("CaveLocation"))
        {
            musicClip = audioManagerScript.CaveMusic;
            audioManagerScript.ChangeMusic(musicClip);
            player.GetComponent<PlayerScript>().location = "Cave";
        }
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
            currentEnemies.Add(Instantiate(enemiesPrefab[Random.Range(0, enemiesPrefab.Count)], point.transform.position, rotation:point.transform.rotation));
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
