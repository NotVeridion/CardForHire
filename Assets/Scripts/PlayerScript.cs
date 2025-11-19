using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public float playerMoveSpeed;
    public float playerHealth;
    public float dashPower;
    public float dashDuration;
    public float currentDashCooldown;
    public float dashCooldown;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        gunSpriteRenderer = GameObject.FindWithTag("Gun").GetComponent<SpriteRenderer>();
        dashTrail = GetComponent<TrailRenderer>();
        canDash = true;
        currentDashCooldown = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        movementVector = new Vector3(horizontal, vertical, 0).normalized;

        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        if (!isDashing)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(nameof(Dash));
        }
    }

    void FixedUpdate()
    {
        // if (!isDashing)
        // {
        //     Move();
        // }

        // if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        // {
        //     StartCoroutine(nameof(Dash));
        // }

    }

    void Move()
    {
        rb.linearVelocity = new Vector3(movementVector.x * playerMoveSpeed, movementVector.y * playerMoveSpeed, 0);

        if (vertical != 0 || horizontal != 0)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }
    }

    IEnumerator Dash()
    {
        // While dashing
        canDash = false;
        isDashing = true;
        rb.linearVelocity = new Vector3(movementVector.x * dashPower, movementVector.y * dashPower, 0);
        dashTrail.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        // While cooldown

        isDashing = false;
        currentDashCooldown -= Time.deltaTime;
        dashTrail.emitting = false;

        yield return new WaitForSeconds(dashCooldown);

        // After cooldown

        currentDashCooldown = dashCooldown;
        canDash = true;
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
}
