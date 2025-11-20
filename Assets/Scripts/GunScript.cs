using UnityEngine;
using System.Collections;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canFire = true;
        GetComponent<SpriteRenderer>().sprite = currentGun.gunSprite;
        deckManagerScript = GameObject.FindWithTag("DeckManager").GetComponent<DeckManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCard = deckManagerScript.currentCard;

        currentTime += Time.deltaTime;
        if (currentTime >= 5f)
        {
            Debug.Log("Current card in gun: " + currentCard.number + " " + currentCard.suit);
            currentTime = 0;
        }

        directionToCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angleToCursor = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angleToCursor);

        if (Input.GetButton("Fire1"))
        {
            if (canFire)
            {
                Shoot();
                StartCoroutine(nameof(fireRateHandler));
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
            Quaternion[] equallySpreadRotations = new Quaternion[currentGun.numBulletsInSpread];

            for (int i = 0; i < currentGun.numBulletsInSpread; i++)
            {
                float angle = -currentGun.spreadRange + currentGun.spreadRange*2 * i / currentGun.numBulletsInSpread;
                Quaternion bulletRot = Quaternion.Euler(Vector3.forward * angle);
                GameObject bulletObj = Instantiate(bullet, bulletSpawner.transform.position, transform.rotation * bulletRot);
                SetBulletData(bulletObj);
            }
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
}
