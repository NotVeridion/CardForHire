using UnityEngine;

public class PickupSpawnerScript : MonoBehaviour
{
    public GameObject pickupPrefab;

    public float maxTime;
    float spawnTimer;

    public bool empty;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject pickup = Instantiate(pickupPrefab, transform);
        pickup.transform.position = new Vector3(transform.position.x, transform.position.y+.2f);
        empty = false;
        spawnTimer = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (empty)
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0)
            {
                GameObject pickup = Instantiate(pickupPrefab, transform);
                pickup.transform.position = new Vector3(transform.position.x, transform.position.y + .2f);
                empty = false;
                spawnTimer = maxTime;
            }
        }
    }
}
