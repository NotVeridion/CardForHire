using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject playerObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -10);
        transform.rotation = Quaternion.identity;
    }
}
