using UnityEngine;

public class BossAttackState : BossState
{
    public BossAttackState(BossScript boss) : base(boss) {}

    Vector3 centerPos;
    Vector3 newPos;
    public override void Enter()
    {
        boss.gun.currentGun = boss.gun.bossShotgun;
        boss.gun.canFire = true;
        centerPos = boss.positionCenter.transform.position;
        newPos = new Vector3(centerPos.x, centerPos.y, -1);
        boss.sliderObj.SetActive(true);
    }

    public override void Exit()
    {
        boss.gun.canFire = false;
    }

    public override void Tick()
    {
        Vector3 centerPos = boss.positionCenter.transform.position;
        Vector3 newPos = new Vector3(centerPos.x, centerPos.y, -1);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, Time.deltaTime * 1.2f);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 15, Time.deltaTime * 1.2f);
        Camera.main.GetComponent<CameraScript>().inFinalBoss = true;

        if (boss.distanceToPlayer > boss.detectionRange)
        {
            Vector2 direction = (boss.player.transform.position - boss.transform.position).normalized;
            boss.rb.linearVelocity = direction * boss.bossMoveSpeed;
        }
        else
        {
            boss.rb.linearVelocity = Vector2.zero;
            boss.gun.Shoot();
        }
    }
}