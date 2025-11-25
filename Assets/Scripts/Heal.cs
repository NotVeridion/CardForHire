using UnityEngine;

public class Heal : MonoBehaviour
{
    PlayerScript player;
    [SerializeField]GameObject notification;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && notification.activeSelf)
        {
            player.Heal(1000);
        }
    }
}
