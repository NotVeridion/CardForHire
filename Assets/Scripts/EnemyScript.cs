using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float enemyMoveSpeed;
    public float enemyBulletSpeed;
    public float enemyShootPauseTime;
    public float enemyHP;
    public float enemyBulletDamage;
    public float detectPlayerRange;
    public float attackPlayerRange;
    public float bulletDuration;
    
    public PlayerScript player;
    public EnemyBulletScript bullet;
    
    private float enemyShootTimer = 0f;
    
    private enum EnemyState
    {
        Idle,
        ChasingPlayer,
        ShootingFromRange
    }
    private EnemyState currentState;
    
    private void takeDamage(float damage)
    {
        enemyHP -= damage;
        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletScript playerBullet = collision.gameObject.GetComponent<BulletScript>();
            takeDamage(playerBullet.bulletDamage);
            Destroy(collision.gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Idle;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        
        if (currentState == EnemyState.Idle)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectPlayerRange)
            {
                currentState = EnemyState.ChasingPlayer;
            }
        }
        else if (currentState == EnemyState.ChasingPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);

            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= attackPlayerRange)
            {
                currentState = EnemyState.ShootingFromRange;
            }
            else if (distanceToPlayer > detectPlayerRange)
            {
                currentState = EnemyState.Idle;
            }

        }
        else if (currentState == EnemyState.ShootingFromRange)
        {
            enemyShootTimer += Time.deltaTime;
            if (enemyShootTimer >= enemyShootPauseTime)
            {
                EnemyBulletScript newBullet = Instantiate(bullet);
                newBullet.transform.position = transform.position;


                var distance = (player.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(distance.y, distance.x) * (180 / Mathf.PI);
                newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                newBullet.bulletMoveSpeed = enemyBulletSpeed;
                newBullet.bulletDamage = enemyBulletDamage;
                newBullet.bulletDuration = bulletDuration;

                enemyShootTimer = 0f;
            }


            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > attackPlayerRange)
            {
                currentState = EnemyState.ChasingPlayer;
            }
        }
    }
}
