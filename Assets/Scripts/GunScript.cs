using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class GunScript : MonoBehaviour
{
    public Gun currentGun;
    public GameObject bulletSpawner;
    public GameObject bullet;
    private Card currentCard;
    private DeckManagerScript deckManagerScript;
    private Vector3 directionToCursor;
    private float angleToCursor;
    private bool canFire;
    private float currentTime;
    private AudioManagerScript audioManagerScript;
    private SpriteRenderer gunSprite;
    private SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canFire = true;
        gunSprite = GetComponent<SpriteRenderer>();
        playerSprite = GetComponentInParent<SpriteRenderer>();

        gunSprite.sprite = currentGun.gunSprite;
        currentGun = Instantiate(currentGun);
        deckManagerScript = GameObject.FindWithTag("DeckManager").GetComponent<DeckManagerScript>();
        audioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerScript player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        if (player != null && player.isMovementLocked)
        {
            return;
        }
        currentCard = deckManagerScript.currentCard;

        // Check if using mouse to aim or arrow keys
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            directionToCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            angleToCursor = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angleToCursor);
        }

        if (Input.GetButton("Fire1"))
        {
            if (canFire)
            {
                Shoot();
                StartCoroutine(nameof(fireRateHandler));
            }
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (playerSprite.flipX)
            {
                playerSprite.flipX = true;
            }
            if (!gunSprite.flipY)
            {
                gunSprite.flipY = true;
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            if (canFire)
            {
                Shoot();
                StartCoroutine(nameof(fireRateHandler));
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            if (canFire)
            {
                Shoot();
                StartCoroutine(nameof(fireRateHandler));
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (playerSprite.flipX)
            {
                playerSprite.flipX = false;
            }
            if (gunSprite.flipY)
            {
                gunSprite.flipY = false;
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (canFire)
            {
                Shoot();
                StartCoroutine(nameof(fireRateHandler));
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            if (canFire)
            {
                Shoot();
                StartCoroutine(nameof(fireRateHandler));
            }
        }
    }

    void Shoot()
    {
        audioManagerScript.PlayRandomSFX(audioManagerScript.Shooting);
        if (currentGun.isSingleShot)
        {
            GameObject bulletObj = Instantiate(bullet, bulletSpawner.transform.position, transform.rotation);
            SetBulletData(bulletObj);
        }
        else if (currentGun.isSpreadShot)
        {
            Quaternion[] equallySpreadRotations = new Quaternion[currentGun.numBulletsInSpread];

            for (int i = 0; i < currentGun.numBulletsInSpread; i++)
            {
                float angle = -currentGun.spreadRange + currentGun.spreadRange*2 * i / currentGun.numBulletsInSpread;
                Quaternion bulletRot = Quaternion.Euler(Vector3.forward * angle);
                GameObject bulletObj = Instantiate(bullet, bulletSpawner.transform.position, transform.rotation * bulletRot);
                SetBulletData(bulletObj);
            }

            // Add a singular bullet that flies towards direction
            GameObject midBullet = Instantiate(bullet, bulletSpawner.transform.position, transform.rotation);
            SetBulletData(midBullet);
        }
    }

    void SetBulletData(GameObject bullet)
    {
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.bulletMoveSpeed = currentGun.bulletMoveSpeed;
        bulletScript.bulletDamage = currentGun.damage;
        bulletScript.bulletDuration = currentGun.bulletDuration;
        bulletScript.applyCardToBullet(currentCard);
    }

    IEnumerator fireRateHandler()
    {
        canFire = false;
        
        yield return new WaitForSeconds(1 / currentGun.fireRate);

        canFire = true;
    }

    public void setCurrentCard(Card card)
    {
        currentCard = card;
    }

    public void RestoreFire()
    {
        canFire = true;
    }
}
