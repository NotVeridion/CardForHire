using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool canFire;
    public GameObject bullet;
    [SerializeField] float turretDamage;
    [SerializeField] float turretMoveSpeed;
    [SerializeField] float turretFireRate;
    [SerializeField] float turretDuration;
    void Start()
    {
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canFire)
        {
            GameObject bulletObj = Instantiate(bullet, transform.position, transform.rotation);
            EnemyBulletScript bulletScript = bulletObj.GetComponent<EnemyBulletScript>();
            bulletScript.bulletDamage = turretDamage;
            bulletScript.bulletMoveSpeed = turretMoveSpeed;
            bulletScript.bulletDuration = turretDuration;

            StartCoroutine(fireRateHandler());
        }
    }

    IEnumerator fireRateHandler()
    {
        canFire = false;
        
        yield return new WaitForSeconds(1 / turretFireRate);

        canFire = true;
    }
}
