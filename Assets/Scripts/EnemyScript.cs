using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float enemyMoveSpeed;
    public float enemyBulletSpeed;
    public float enemyHP;
    public float enemyBulletDamage;
    public float detectPlayerRange;
    public float attackPlayerRange;
    public float bulletDuration;
    public float idleStopWalkingTime = 5f;
    
    
    public EnemyGunScript gun;
    
    public bool isStunned;
    public bool doesRoamAround = true;
    
    private GameObject player;

    private float idleStopWalkingTimer = 0f;
    private float chaseWhenShotTimer = 0f;
    private float radius = 1f;
    
    private bool isCurrentlyRoaming = false;
    
    private Vector3 nextIdlePosition;
    
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
            if (gameObject != null)
            {
                Destroy(gameObject);
                PlayerScript player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
                if (player != null)
                    player.playerCash += 25;
                EnemyDefeatTracker.Instance.NotifyEnemyDefeated("Enemy");
            }
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
        player = GameObject.FindWithTag("Player");
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
            GetComponent<Animator>().SetBool("isMoving", false);
            gun.isPointingAtPlayer = false;
            gun.isShooting = false;
            if (doesRoamAround)
            {
                idleStopWalkingTimer += Time.deltaTime;
                if (idleStopWalkingTimer >= idleStopWalkingTime)
                {
                    if (!isCurrentlyRoaming)
                    {
                        nextIdlePosition = (Random.insideUnitCircle * radius) + (Vector2)transform.position;
                        Vector3 checkDirection = (nextIdlePosition - transform.position).normalized;
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, checkDirection, Vector2.Distance(transform.position, nextIdlePosition) + 1.4f, LayerMask.GetMask("Wall"));
                        
                        if (hit)
                        {
                            nextIdlePosition = transform.position;
                        }
                    }
                    
                    isCurrentlyRoaming = true;
                    
                    Vector3 distanceToNextPoint = nextIdlePosition - transform.position;
                    Vector3 direction = (nextIdlePosition - transform.position).normalized;

                    if (distanceToNextPoint.magnitude >= 0.1f)
                    {
                        transform.position += direction * (enemyMoveSpeed / 2.5f) * Time.deltaTime;
                        GetComponent<Animator>().SetBool("isMoving", true);
                    }
                    else
                    {
                        idleStopWalkingTimer = 0f;
                        isCurrentlyRoaming = false;
                    }
                }

            }
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectPlayerRange)
            {
                currentState = EnemyState.ChasingPlayer;
            }
            
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
            isCurrentlyRoaming = false;
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
            isCurrentlyRoaming = false;
            chaseWhenShotTimer += Time.deltaTime;
            if ((chaseWhenShotTimer >= 6f) && (distanceToPlayer > detectPlayerRange))
            {
                currentState = EnemyState.Idle;
                chaseWhenShotTimer = 0f;
            }
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
