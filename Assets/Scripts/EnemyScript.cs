using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float enemyMoveSpeed;
    public float enemyBulletSpeed;
    //public float enemyShootPauseTime;
    public float enemyHP;
    public float enemyBulletDamage;
    public float detectPlayerRange;
    public float attackPlayerRange;
    public float bulletDuration;
    
    public PlayerScript player;
    public EnemyGunScript gun;
    //public EnemyBulletScript bullet;
    public bool isStunned;
    
    //private float enemyShootTimer = 0f;
    
    private enum EnemyState
    {
        Idle,
        ChasingPlayer,
        ShotByPlayer,
        ShootingFromRange
    }
    private EnemyState currentState;
    
    public void TakeDamage(float damage)
    {
        enemyHP -= damage;
        if (enemyHP <= 0)
        {
            Destroy(gameObject);
            EnemyDefeatTracker.Instance.NotifyEnemyDefeated("Enemy");
        }
        if (currentState == EnemyState.Idle)
        {
            currentState = EnemyState.ShotByPlayer;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Idle;
    }

    void Update()
    {
        if (player.transform.position.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
             GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void FixedUpdate()
    {
        // If stunned, don't do anything
        if (isStunned)
        {
            return;
        }
        
        if (currentState == EnemyState.Idle)
        {
            gun.isPointingAtPlayer = false;
            gun.isShooting = false;
            
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectPlayerRange)
            {
                currentState = EnemyState.ChasingPlayer;
            }
            GetComponent<Animator>().SetBool("isMoving", false);
        }
        else if (currentState == EnemyState.ChasingPlayer)
        {
            gun.isPointingAtPlayer = false;
            gun.isShooting = false;
            
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
            GetComponent<Animator>().SetBool("isMoving", true);
        }
        else if (currentState == EnemyState.ShotByPlayer)
        {
            gun.isPointingAtPlayer = false;
            gun.isShooting = false;
            
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);

            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= attackPlayerRange)
            {
                currentState = EnemyState.ShootingFromRange;
            }
            GetComponent<Animator>().SetBool("isMoving", true);
        }
        else if (currentState == EnemyState.ShootingFromRange)
        {
            gun.isPointingAtPlayer = true;
            gun.isShooting = true;


            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > attackPlayerRange)
            {
                currentState = EnemyState.ChasingPlayer;
            }
            GetComponent<Animator>().SetBool("isMoving", false);
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
                enemyMoveSpeed += amt;
                break;
        }
    }
}
