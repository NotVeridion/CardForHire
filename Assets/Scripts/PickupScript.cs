using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] GameObject spriteIcon;
    Vector3 rotationSpeed = new Vector3(0, 75f, 0);

    public float value;
    public float duration;

    // Update is called once per frame
    void Update()
    {
        spriteIcon.transform.transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponentInParent<PickupSpawnerScript>().empty = true;
            Destroy(gameObject);
        }
    }
}
