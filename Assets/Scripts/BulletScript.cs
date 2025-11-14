using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float bulletMoveSpeed;
    public float bulletDamage;
    public float bulletDuration;
    public PlayerScript playerScript;
    private Vector3 prevPosition;
    private RaycastHit2D[] prevHits;
    private Rigidbody2D rb;
    bool isHealing;
    bool isPiercing;
    bool isKnockback;
    bool isRicochet;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * bulletMoveSpeed;
        Destroy(gameObject, bulletDuration);

        prevPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // Draw raycast to previous position after a few frames
        if (Time.frameCount % 5 == 0)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, prevPosition);
            if (isHealing)
            {
                handleHealing(hits);
            }
            else if (isPiercing)
            {
                handlePiercing(hits);
            }
            else if (isKnockback)
            {
                handleKnockback(hits);
            }
            else if (isRicochet)
            {
                handleBouncing(hits);
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
                break;
            case Card.Suit.Spades:
                isRicochet = true;
                break;
        }
    }

    void handleHealing(RaycastHit2D[] raycast)
    {
        
    }

    void handlePiercing(RaycastHit2D[] raycast)
    {

    }

    void handleKnockback(RaycastHit2D[] raycast)
    {

    }
    
    void handleBouncing(RaycastHit2D[] raycast)
    {
        
    }
}
