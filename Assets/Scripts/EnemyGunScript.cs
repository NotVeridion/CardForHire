using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyGunScript : MonoBehaviour
{
    public Gun currentGun;
    public GameObject bulletSpawner;
    public GameObject bullet;
    public GameObject player;
    public bool isPointingAtPlayer;
    public bool isShooting;
    private Card currentCard;
    private DeckManagerScript deckManagerScript;
    private Vector3 directionToPlayer;
    private float angleToPlayer;
    private bool canFire;
    private float currentTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canFire = true;
        GetComponent<SpriteRenderer>().sprite = currentGun.gunSprite;
        deckManagerScript = GameObject.FindWithTag("DeckManager").GetComponent<DeckManagerScript>();
        isPointingAtPlayer = false;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isPointingAtPlayer)
        {
            directionToPlayer = (player.transform.position - transform.position).normalized;
            angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);
        }
        if (isShooting)
        {
            if (canFire)
            {
                Shoot();
                StartCoroutine(nameof(fireRateHandler));
            }
        }

        if (player.transform.position.x < transform.position.x)
        {
            
            if (isPointingAtPlayer)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().flipY = true;
            }
        }
        else
        {
            if (isPointingAtPlayer)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().flipY = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(Vector3.forward);
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().flipY = false;
            }
        }
        
    }
    
    void Shoot()
    {
        if (currentGun.isSingleShot)
        {
            GameObject bulletObj = Instantiate(bullet, bulletSpawner.transform.position, transform.rotation);
            SetBulletData(bulletObj);
        }
        else if (currentGun.isSpreadShot)
        {
            Debug.Log("SHOOT!");
            Quaternion[] equallySpreadRotations = new Quaternion[currentGun.numBulletsInSpread];

            for (int i = 0; i < currentGun.numBulletsInSpread; i++)
            {
                float angle = -currentGun.spreadRange + currentGun.spreadRange*2 * i / currentGun.numBulletsInSpread;
                equallySpreadRotations[i] = Quaternion.Euler(Vector3.forward * angle);
            }
            
            for (int i = 0; i < currentGun.numBulletsInSpread; i++)
            {
                GameObject bulletObj = Instantiate(bullet, bulletSpawner.transform.position, transform.rotation * equallySpreadRotations[i]);
                SetBulletData(bulletObj);
            }
        }
    }

    void SetBulletData(GameObject bullet)
    {
        EnemyBulletScript bulletScript = bullet.GetComponent<EnemyBulletScript>();
        bulletScript.bulletMoveSpeed = currentGun.bulletMoveSpeed;
        bulletScript.bulletDamage = currentGun.damage;
        bulletScript.bulletDuration = currentGun.bulletDuration;
    }

    IEnumerator fireRateHandler()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / currentGun.fireRate);

        canFire = true;
    }
}
