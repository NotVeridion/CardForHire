using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float bulletMoveSpeed;
    public float bulletDamage;
    public float bulletDuration;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * bulletMoveSpeed;
        Destroy(gameObject, bulletDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
