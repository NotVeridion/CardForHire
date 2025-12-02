using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public float playerMoveSpeed;
    public float playerHealth;
    public int playerCash;
    public float dashPower;
    public float dashDuration;
    private float currentDashDuration;
    public float dashCooldown;
    [HideInInspector]
    public float currentDashCooldown;
    [HideInInspector]
    public string location;
    private GunScript gunScript;
    private TrailRenderer dashTrail;
    private Rigidbody2D rb;
    private bool canDash;
    private bool isDashing;
    private float vertical;
    private float horizontal;
    private Vector3 movementVector;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer gunSpriteRenderer;
    private AudioManagerScript audioManagerScript;
    private NPCScript npcInRange;
    public bool isMovementLocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        gunSpriteRenderer = GameObject.FindWithTag("Gun").GetComponent<SpriteRenderer>();
        audioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManagerScript>();
        gunScript = GetComponentInChildren<GunScript>();
        dashTrail = GetComponent<TrailRenderer>();
        canDash = true;
        currentDashDuration = dashDuration;
        currentDashCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMovementLocked)
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            vertical = 0;
            horizontal = 0;
        }

        movementVector = new Vector3(horizontal, vertical, 0).normalized;

        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.E) && npcInRange != null)
        {
            npcInRange.Interact(); // Start dialogue
        }
        if (transform.position.x <= cursorPos.x) // Cursor on right of player
        {
            playerSpriteRenderer.flipX = false;
            gunSpriteRenderer.flipY = false;
        }
        else
        {
            playerSpriteRenderer.flipX = true;
            gunSpriteRenderer.flipY = true;
        }

        // DASH has two parts: dash duration + dash cooldown
        // Dash duration -> Amount of time the velocity of player is set in direction of movement for
        //      - Movement is disabled during dash duration
        // Dash cooldown -> Amount of time the dash is disabled. For UI, grab this value from the player every frame.
        //      - Once duration is over, dash cooldown begins to count down from dashCooldown to 0
        //      - While dash is ready, the cd stays at a value of 0

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            // Starting up the dash
            canDash = false;
            isDashing = true;
            currentDashCooldown = dashCooldown;
            rb.linearVelocity = new Vector3(movementVector.x * dashPower, movementVector.y * dashPower, 0);
            dashTrail.emitting = true;

            audioManagerScript.PlayOneShotSFX(audioManagerScript.Dash);
        }

        if (!isDashing)
        {
            Move();
        }

        if (isDashing)
        {
            currentDashDuration -= Time.deltaTime;
            if (currentDashDuration <= 0)
            {
                currentDashDuration = dashDuration;
                isDashing = false;
                dashTrail.emitting = true;
            }
        }
        else if (canDash == false)
        {
            dashTrail.emitting = false;
            currentDashCooldown -= Time.deltaTime;
            if (currentDashCooldown < 0)
            {
                currentDashCooldown = 0;
                canDash = true;
            }
        }
    }

    void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("AttackSpeedPickup") || other.gameObject.CompareTag("DamagePickup")
            || other.gameObject.CompareTag("DashDistancePickup") || other.gameObject.CompareTag("MovementSpeedPickup"))
        {
            PickupScript script = other.gameObject.GetComponent<PickupScript>();
            if (other.gameObject.CompareTag("AttackSpeedPickup"))
            {
                StartCoroutine(applyAttackSpeedBuff(script.value, script.duration));
            }
            if (other.gameObject.CompareTag("DamagePickup"))
            {
                StartCoroutine(applyDamageBuff(script.value, script.duration));
            }
            if (other.gameObject.CompareTag("DashDistancePickup"))
            {
                StartCoroutine(applyDashDistanceBuff(script.value, script.duration));
            }
            if (other.gameObject.CompareTag("MovementSpeedPickup"))
            {
                StartCoroutine(applyMovementSpeedBuff(script.value, script.duration));
            }

            audioManagerScript.PlayOneShotSFX(audioManagerScript.Pickup);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            EnemyBulletScript enemyBullet = other.gameObject.GetComponent<EnemyBulletScript>();
            TakeDamage(enemyBullet.bulletDamage);
            audioManagerScript.PlayOneShotSFX(audioManagerScript.Hit);

            Destroy(other.gameObject);
        }
        if (other.CompareTag("NPCInteraction"))
        {
            npcInRange = other.GetComponentInParent<NPCScript>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPCInteraction"))
        {
            npcInRange = null;
        }
    }
    void Move()
    {
        rb.linearVelocity = new Vector3(movementVector.x * playerMoveSpeed, movementVector.y * playerMoveSpeed, 0);

        if (vertical != 0 || horizontal != 0)
        {
            playerAnimator.SetBool("isMoving", true);

            if (!audioManagerScript.SFXSource.isPlaying){
                audioManagerScript.PlayRandomSFX(audioManagerScript.WalkingGrass);
            }
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }
    }

    public void TakeDamage(float dmg)
    {
        playerHealth -= dmg;
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
    }

    public void Heal(float amt)
    {
        playerHealth += amt;
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
    }

    public void ChangeStat(string stat, float amt)
    {
        switch (stat)
        {
            case "FireRate":
                gunScript.currentGun.fireRate += amt;
                break;
            case "EnergyRegain":
                currentDashCooldown += amt;
                if (currentDashCooldown <= 0)
                {
                    currentDashCooldown = 0;
                }
                break;
            case "Damage":
                gunScript.currentGun.damage += amt;
                break;
            case "Bullet Count":
                gunScript.currentGun.numBulletsInSpread += (int) amt;
                break;
        }
    }

    IEnumerator applyAttackSpeedBuff(float amt, float duration)
    {
        gunScript.currentGun.fireRate += amt;
        
        yield return new WaitForSeconds(duration);

        gunScript.currentGun.fireRate -= amt;
    }
    IEnumerator applyDamageBuff(float amt, float duration)
    {
        gunScript.currentGun.damage += amt;
        
        yield return new WaitForSeconds(duration);

        gunScript.currentGun.damage -= amt;
    }
    IEnumerator applyDashDistanceBuff(float amt, float duration)
    {
        dashDuration += amt;
        
        yield return new WaitForSeconds(duration);

        dashDuration -= amt;
    }
    IEnumerator applyMovementSpeedBuff(float amt, float duration)
    {
        playerMoveSpeed += amt;
        
        yield return new WaitForSeconds(duration);

        playerMoveSpeed -= amt;
    }
}
