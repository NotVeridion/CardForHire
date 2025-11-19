using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class BulletScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float bulletMoveSpeed;
    public float bulletDamage;
    public float bulletDuration;
    public float raycastLength;
    private float raycastFreq;
    private PlayerScript playerScript;
    private List<GameObject> prevHits; // Only for piercing bullets
    private Rigidbody2D rb;
    private bool isHealing;
    private bool isPiercing;
    private bool isKnockback;
    public float knockbackForce = 2;
    private bool isRicochet;
    private float currentTime;
    void Start()
    {
        // Piercing
        raycastFreq = 0.1f; 
        prevHits = new List<GameObject>();

        Destroy(gameObject, bulletDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * bulletMoveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject other_obj = other.gameObject;
        if (isHealing)
        {
            handleHealing(other_obj);
        }
        else if (isPiercing)
        {
            handlePiercing(other_obj);
        }
        else if (isKnockback)
        {
            // TODO: Implement piercing
            handleKnockback(other_obj);
        }
        else if (isRicochet)
        {
            handleRicochet(other);
        }
    }

    public void applyCardToBullet(Card card)
    {
        bulletDamage += card.number; // Number adds to current bullet damage

        switch (card.suit)
        {
            case Card.Suit.Hearts:
                isHealing = true;
                break;
            case Card.Suit.Diamonds:
                isPiercing = true;
                break;
            case Card.Suit.Clubs:
                isKnockback = true;
                knockbackForce += 0.5f * card.number;
                break;
            case Card.Suit.Spades:
                isRicochet = true;
                break;
        }
    }

    void handleHealing(GameObject other_obj)
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        if (other_obj.CompareTag("TestDummy") || other_obj.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit");
            other_obj.GetComponent<TestDummy>().TakeDamage(bulletDamage);
            playerScript.Heal(bulletDamage);
            Destroy(gameObject);
        }
        
    }

    void handlePiercing(GameObject other_obj)
    {
        if (prevHits.Contains(other_obj))
        {
            return;
        }
        else if (prevHits.Contains(other_obj) == false && other_obj.CompareTag("TestDummy") || other_obj.CompareTag("Enemy"))
        {
            Debug.Log("Piercing hit");
            other_obj.GetComponent<TestDummy>().TakeDamage(bulletDamage);
            prevHits.Add(other_obj);
        }
    }

    void handleKnockback(GameObject other_obj)
    {
        // Needs to knock enemy back in opposite direction between bullet and enemy
        if (other_obj.CompareTag("TestDummy") || other_obj.CompareTag("Enemy"))
        {
            other_obj.GetComponent<Rigidbody2D>().AddForce((other_obj.transform.position - transform.position).normalized, ForceMode2D.Impulse);
            other_obj.GetComponent<TestDummy>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
    
    void handleRicochet(Collider2D other)
    {
        // // Ball becomes bouncy and faster. Bounces off of any surface and also extends the bullet duration initially as well as each time it hits an enemy.
        // if (other.gameObject.CompareTag("TestDummy") || other.gameObject.CompareTag("Enemy"))
        // {
        //     Vector2 ricochetDirection = Vector2.Reflect(transform.position, other.transform.position);
        //     float rotAngle = Mathf.Atan2(ricochetDirection.y, ricochetDirection.x) * Mathf.Rad2Deg - 90;
        //     transform.rotation *= Quaternion.Euler(0, 0, rotAngle);

        //     other.GetComponent<TestDummy>().TakeDamage(bulletDamage);
        // }
    }
}
