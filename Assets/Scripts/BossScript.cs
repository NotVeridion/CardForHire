using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float bossMoveSpeed;
    [HideInInspector]
    public BossGunScript gun;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public float distanceToPlayer;
    [HideInInspector]
    public Rigidbody2D rb;
    public float detectionRange;
    public GameObject sliderObj;

    // State Machine
    public BossStateMachine StateMachine { get; private set; }

    // States
    public BossIdleState IdleState { get; private set; }
    public BossAttackState AttackState { get; private set; }
    public BossSpecialAttackState SpecialAttackState { get; private set; }
    public BossFinalState FinalState { get; private set; }

    // Positions
    public GameObject[] positionObjects;
    public GameObject positionCenter;
    public GameObject positionStart;

    private Slider healthSlider;

    void Awake()
    {
        StateMachine = new BossStateMachine();
        IdleState = new BossIdleState(this);
        AttackState = new BossAttackState(this);
        SpecialAttackState = new BossSpecialAttackState(this);
        FinalState = new BossFinalState(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<BossGunScript>();
        healthSlider = sliderObj.GetComponent<Slider>();
        healthSlider.maxValue = maxHealth;

        StateMachine.Initialize(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth;

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (player.transform.position.x <= transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            gun.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (player.transform.position.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            gun.GetComponent<SpriteRenderer>().flipY = false;
        }

        StateMachine.Update();

        if (currentHealth <= (maxHealth - maxHealth/2) && StateMachine.CurrentState == SpecialAttackState)
        {
            // Destroy all bullets currently on screen
            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
            {
                Destroy(bullet);
            }

            StateMachine.ChangeState(FinalState);
            gun.GetComponent<SpriteRenderer>().sprite = null;
        }
        else if (currentHealth <= (maxHealth - maxHealth/3) && StateMachine.CurrentState == AttackState)
        {
            StateMachine.ChangeState(SpecialAttackState);
            gun.GetComponent<SpriteRenderer>().sprite = gun.currentGun.gunSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (StateMachine.CurrentState == IdleState)
            {
                StateMachine.ChangeState(AttackState);
            }
            
            BulletScript playerBullet = collision.gameObject.GetComponent<BulletScript>();
            TakeDamage(playerBullet.bulletDamage);

            if (!playerBullet.isPiercing)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            AudioManagerScript musicScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManagerScript>();
            musicScript.ChangeMusic(musicScript.EndMusic);
            SceneManager.LoadScene("End");
        }
    }
}
