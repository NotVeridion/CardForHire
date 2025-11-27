using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float bulletMoveSpeed;
    public float bulletDamage;
    public float bulletDuration;
    public float raycastLength;
    public float raycastFreq;
    public List<GameObject> debuffs;
    public Card card;
    private PlayerScript playerScript;
    private List<int> prevHits; // For piercing bullets
    private bool isHealing;
    [HideInInspector]
    public bool isPiercing;
    private bool isKnockback;
    public float knockbackForce = 2;
    private bool isHoming;
    public float homingRadius = 4f;
    public float rotationSpeed = 1f;
    private GameObject target;
    private float currentTime;
    void Start()
    {

        // Piercing
        raycastFreq = 0.1f; 
        prevHits = new List<int>();

        Destroy(gameObject, bulletDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * bulletMoveSpeed * Time.deltaTime;

        if (isHoming)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, homingRadius, transform.right, homingRadius);
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("TestDummy"))
                {
                    Vector3 direction = hit.collider.transform.position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.position = Vector2.MoveTowards(transform.position, hit.collider.transform.position, bulletMoveSpeed * Time.deltaTime);
                    Quaternion newRotation = Quaternion.Euler(Vector3.forward * angle);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawSphere(transform.position, homingRadius);
    // }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.right, Color.blue);
        Debug.DrawRay(transform.position, transform.up, Color.red);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject other_obj = other.gameObject;
        if (other_obj.CompareTag("TestDummy") || other_obj.CompareTag("Enemy"))
        {
            if (isHealing)
            {
                playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
                playerScript.Heal(bulletDamage);
            }
            else if (isPiercing)
            {
                if (prevHits.Contains(other_obj.GetInstanceID()))
                {
                    return;
                }
                else
                {
                    prevHits.Add(other_obj.GetInstanceID());
                }
            }
            else if (isKnockback)
            {
                // Needs to knock enemy back in opposite direction between bullet and enemy

                if (other_obj.TryGetComponent(out EnemyScript eScript))
                {
                    other_obj.GetComponent<Rigidbody2D>().AddForce(transform.right * 2, ForceMode2D.Impulse);
                }
                else if (other_obj.TryGetComponent(out TestDummy script))
                {
                    other_obj.GetComponent<Rigidbody2D>().AddForce(transform.right * 2, ForceMode2D.Impulse);
                }
            }

            applyDebuffToTarget(other_obj);
        }
        else if (other_obj.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void applyCardToBullet(Card card)
    {
        bulletDamage += card.number; // Number adds to current bullet damage
        this.card = card;

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
                isHoming = true;
                break;
        }
    }

    public void applyDebuffToTarget(GameObject target)
    {
        if (card.slow)
        {
            foreach (GameObject debuff in debuffs)
            {
                if (debuff.name == "Slow")
                {
                    // If already slowed, remove slow and reapply
                    foreach (Transform child in target.transform)
                    {
                        if (child.TryGetComponent(out SlowEffect script))
                        {
                            target.GetComponent<EnemyScript>().ChangeStat("Slow", script.amt);
                            Destroy(child.gameObject);
                        }
                    }

                    GameObject debuffObj = Instantiate(debuff, target.transform.position, target.transform.rotation);
                    debuffObj.transform.parent = target.transform; // Debuff becomes child of target
                    debuffObj.GetComponent<SlowEffect>().target = target.GetComponent<EnemyScript>();
                    break;
                }
            }
        }
        if (card.bleed)
        {
            foreach (GameObject debuff in debuffs)
            {
                if (debuff.name == "Bleed")
                {
                    GameObject debuffObj = Instantiate(debuff, target.transform.position, target.transform.rotation);
                    debuffObj.transform.parent = target.transform; // Debuff becomes child of target
                    debuffObj.GetComponent<BleedEffect>().target = target.GetComponent<EnemyScript>();
                    break;
                }
            }
        }
        if (card.knockOut)
        {
            foreach (GameObject debuff in debuffs)
            {
                if (debuff.name == "Knockout")
                {
                    float prob = debuff.GetComponent<KnockoutEffect>().probability;

                    // Roll for chance to NOT happen
                    float randomNum = Random.Range(0, 101);
                    if (randomNum <= 100 * (1 - prob))
                    {
                        break;
                    }

                    // If already stunned, remove and reapply
                    foreach (Transform child in target.transform)
                    {
                        if (child.TryGetComponent(out SlowEffect script))
                        {
                            target.GetComponent<EnemyScript>().isStunned = false;
                            Destroy(child.gameObject);
                        }
                    }

                    GameObject debuffObj = Instantiate(debuff, target.transform.position, target.transform.rotation);
                    debuffObj.transform.parent = target.transform; // Debuff becomes child of target
                    debuffObj.GetComponent<KnockoutEffect>().target = target.GetComponent<EnemyScript>();
                    break;
                }
            }
        }
        if (card.faster)
        {
            foreach (GameObject debuff in debuffs)
            {
                if (debuff.name == "FireRate")
                {
                    GameObject debuffObj = Instantiate(debuff, playerScript.transform.position, playerScript.transform.rotation);
                    debuffObj.transform.parent = playerScript.transform; // Debuff becomes child of target
                    debuffObj.GetComponent<FireRateEffect>().player = playerScript;
                    break;
                }
            }
        }
        if (card.energyRegain)
        {
            foreach (GameObject debuff in debuffs)
            {
                if (debuff.name == "EnergyRegain")
                {
                    GameObject debuffObj = Instantiate(debuff, playerScript.transform.position, playerScript.transform.rotation);
                    debuffObj.transform.parent = playerScript.transform; // Debuff becomes child of target
                    debuffObj.GetComponent<EnergyRegainEffect>().player = playerScript;
                    break;
                }
            }
        }
    }
}
