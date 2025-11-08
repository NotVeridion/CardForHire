using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float playerMoveSpeed;
    private float vertical;
    private float horizontal;
    private Vector3 movementVector;  
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer gunSpriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        gunSpriteRenderer = GameObject.FindWithTag("Gun").GetComponent<SpriteRenderer>();
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
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        transform.position += movementVector * playerMoveSpeed * Time.fixedDeltaTime;

        if (vertical != 0 || horizontal != 0)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }
    }

}
