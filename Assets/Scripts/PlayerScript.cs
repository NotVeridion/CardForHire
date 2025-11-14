using UnityEngine;
using System.Collections;
using TMPro;
using System.Data;

public class PlayerScript : MonoBehaviour
{
    public float playerMoveSpeed;
    public float dashPower;
    public float dashDuration;
    public float dashCooldown;
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
        canDash = true;
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
        canDash = false;
        isDashing = true;

        rb.linearVelocity = new Vector3(movementVector.x * dashPower, movementVector.y * dashPower, 0);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}
