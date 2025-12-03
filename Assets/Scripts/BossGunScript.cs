using UnityEngine;
using System.Collections;

public class BossGunScript : MonoBehaviour
{
    public BossScript boss;
    public Gun bossShotgun;
    public Gun bossPistol;
    public Gun bossFinalGun;

    [SerializeField] GameObject regularBullet;
    [SerializeField] GameObject specialBullet;
    [SerializeField] GameObject bulletSpawner;

    public bool canFire;

    public Gun currentGun;
    public GameObject indicatorPrefab;
    public float indicatorDuration;
    private GameObject currentIndicator;
    private GameObject player;
    private bool isIndicating;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGun = bossShotgun;
        GetComponent<SpriteRenderer>().sprite = currentGun.gunSprite;
        player = GameObject.FindWithTag("Player");
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isIndicating && (boss.StateMachine.CurrentState == boss.AttackState || boss.StateMachine.CurrentState == boss.SpecialAttackState))
        {
            Vector2 dirToPlayer = (player.transform.position - transform.position).normalized;
            float angleToPlayer = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);
        }
    }

    public void Shoot()
    {
        if (!canFire)
        {
            return;
        }

        if (currentGun == bossPistol)
        {
            currentIndicator = Instantiate(indicatorPrefab, transform.position, transform.rotation);
            StartCoroutine(indicatorShot(indicatorDuration));
        }
        else if (currentGun == bossShotgun)
        {
            for (int i = 0; i < currentGun.numBulletsInSpread; i++)
            {
                int randomOffset = Random.Range(0, 50);
                float angle = -currentGun.spreadRange + currentGun.spreadRange*2 * i / currentGun.numBulletsInSpread;
                Quaternion bulletRot = Quaternion.Euler(Vector3.forward * (angle + randomOffset));
                GameObject bulletObj = Instantiate(regularBullet, bulletSpawner.transform.position, transform.rotation * bulletRot);
                SetBulletData(bulletObj);
            }

            // Add a singular bullet that flies towards direction
            
            GameObject midBullet = Instantiate(regularBullet, bulletSpawner.transform.position, transform.rotation * Quaternion.Euler(Vector3.forward *  Random.Range(0, 50)));
            SetBulletData(midBullet);

            StartCoroutine(fireRateHandler());
        }
        else if (currentGun == bossFinalGun)
        {
            GameObject bullet = Instantiate(regularBullet, bulletSpawner.transform.position, transform.rotation * Quaternion.Euler(Vector3.forward *  Random.Range(0, 50)));
            SetBulletData(bullet);

            StartCoroutine(fireRateHandler());
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

    IEnumerator indicatorShot(float indicatorDuration)
    {
        canFire = false;
        isIndicating = true;

        yield return new WaitForSeconds(indicatorDuration);

        isIndicating = false;
        Destroy(currentIndicator);

        GameObject bulletObj = Instantiate(specialBullet, bulletSpawner.transform.position, transform.rotation);
        SetBulletData(bulletObj);

        StartCoroutine(fireRateHandler());
    }
}
