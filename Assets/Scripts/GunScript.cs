using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class GunScript : MonoBehaviour
{
    public Gun currentGun;
    public GameObject bulletSpawner;
    public GameObject bullet;
    private Vector3 directionToCursor;
    private float angleToCursor;
    private bool canFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canFire = true;
        GetComponent<SpriteRenderer>().sprite = currentGun.gunSprite;
    }

    // Update is called once per frame
    void Update()
    {
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
                float angle = -currentGun.spreadRange + currentGun.spreadRange*2 * i/ currentGun.numBulletsInSpread;
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
        BulletScript script = bullet.GetComponent<BulletScript>();
        script.bulletMoveSpeed = currentGun.bulletMoveSpeed;
        script.bulletDamage = currentGun.damage;
        script.bulletDuration = currentGun.bulletDuration;
    }

    IEnumerator fireRateHandler()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / currentGun.fireRate);

        canFire = true;
    }
}
