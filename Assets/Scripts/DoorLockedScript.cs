using UnityEngine;

public class DoorLockedScript : MonoBehaviour
{
    public GameObject info;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        info.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        info.SetActive(false);
    }

}
