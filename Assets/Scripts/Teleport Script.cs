using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] GameObject teleportPosition;
    [SerializeField] GameObject player;
    [SerializeField] GameObject teleportInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
    }
}
