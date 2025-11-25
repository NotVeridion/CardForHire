using TMPro;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    public float dummyHealth;
    public float dummyMovementSpeed;
    public bool isStunned;
    private TextMeshPro statsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statsText = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            statsText.text = "STUNNED";
        }
        else
        {
             statsText.text = "Health: " + dummyHealth.ToString() + "\n"
                        + "Movement Speed: " + dummyMovementSpeed.ToString() + "\n";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletScript playerBullet = collision.gameObject.GetComponent<BulletScript>();
            TakeDamage(playerBullet.bulletDamage);

            if (!playerBullet.isPiercing)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        dummyHealth -= dmg;
        if (dummyHealth <= 0)
        {
            dummyHealth = 100;
        }
    }

    // Amount will be positive or negative based on the instantiated effect that calls it
    // Has to be += to allow for debuff to be applied then reverted by ApplyEffect() in TimedEffect.cs
    //      and classes deriving it
    public void ChangeStat(string stat, float amt)
    {
        switch (stat)
        {
            case "Slow":
                dummyMovementSpeed += amt;
                break;
        }
    }
}
