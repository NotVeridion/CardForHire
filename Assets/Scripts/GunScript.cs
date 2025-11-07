using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public GunScriptableObject gunScriptableObject;
    public GameObject bulletSpawner;
    public GameObject bullet;
    private Vector3 directionToCursor;
    private float angleToCursor;
    private bool canFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canFire = true;
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
        GameObject bulletObj = Instantiate(bullet, bulletSpawner.transform.position, transform.rotation);

        SetBulletData(bulletObj);
    }
    
    void SetBulletData(GameObject bullet)
    {
        BulletScript script = bullet.GetComponent<BulletScript>();
        script.bulletMoveSpeed = gunScriptableObject.bulletMoveSpeed;
        script.bulletDamage = gunScriptableObject.bulletDamage;
        script.bulletDuration = gunScriptableObject.bulletDuration;
    }
    
    IEnumerator fireRateHandler()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / gunScriptableObject.fireRate);

        canFire = true;
    }
}
