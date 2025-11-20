using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit.Framework.Internal;
using System;
public class BulletScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float bulletMoveSpeed;
    public float bulletDamage;
    public float bulletDuration;
    public float raycastLength;
    private float raycastFreq;
    private PlayerScript playerScript;
    private List<GameObject> prevHits; // For piercing bullets
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
        if (other_obj.CompareTag("TestDummy") || other_obj.CompareTag("Enemy") || other_obj.CompareTag("Wall"))
        {
            if (isHealing)
            {
                playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
                if (other_obj.TryGetComponent<TestDummy>(out TestDummy script))
                {
                    script.TakeDamage(bulletDamage);
                    playerScript.Heal(bulletDamage);
                }
                Destroy(gameObject);
            }
            else if (isPiercing)
            {
                if (prevHits.Contains(other_obj))
                {
                    return;
                }
                else
                {
                    if (other_obj.TryGetComponent<TestDummy>(out TestDummy script))
                    {
                        script.TakeDamage(bulletDamage);
                    }
                    prevHits.Add(other_obj);
                }
            }
            else if (isKnockback)
            {
                // Needs to knock enemy back in opposite direction between bullet and enemy
                if (other_obj.TryGetComponent<TestDummy>(out TestDummy script))
                {
                    script.TakeDamage(bulletDamage);
                    other_obj.GetComponent<Rigidbody2D>().AddForce(transform.right * 2, ForceMode2D.Impulse);
                }

                Destroy(gameObject);
            }
            else if (isRicochet)
            {
                List<ContactPoint2D> contacts = new List<ContactPoint2D>();
                int count = other.GetContacts(contacts);
                if (count > 0)
                {
                    Vector3 reflectDir = Vector3.Reflect(transform.right.normalized, contacts[0].point);
                    float rotAngle = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, rotAngle);
                    transform.position += transform.right * 0.1f;
                }
            }
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
}
