using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] GameObject spriteIcon;
    Vector3 rotationSpeed = new Vector3(0, 75f, 0);

    public float value;
    public float duration;

    [SerializeField] GameUISccript uISccript;

    private void Start()
    {
        uISccript = FindAnyObjectByType<GameUISccript>();
    }


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
            switch (gameObject.tag)
            {
                case "AttackSpeedPickup":
                    uISccript.AttackSpeedDuration(duration);
                    break;

                case "DamagePickup":
                    uISccript.DamageDuration(duration);
                    break;

                case "MovementSpeedPickup":
                    uISccript.MovementDuration(duration);
                    break;

                case "DashDistancePickup":
                    uISccript.DashDistance(duration);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
