using UnityEngine;

public class DoorLockedScript : MonoBehaviour
{
    public GameObject info;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            info.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            info.SetActive(false);
        }
    }

}
