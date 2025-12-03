using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float zoom;
    public bool inFinalBoss;
    private GameObject playerObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerObject != null && !inFinalBoss){
            transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -10);
            transform.rotation = Quaternion.identity;
            Camera.main.orthographicSize = zoom;
        }
    }
}
