using UnityEngine;

public class BossAttackState : BossState
{
    public BossAttackState(BossScript boss) : base(boss) {}

    public override void Enter()
    {
        boss.gun.currentGun = boss.gun.bossShotgun;
        boss.gun.canFire = true;

        Vector3 centerPos = boss.positionCenter.transform.position;
        Camera.main.transform.position = new Vector3(centerPos.x, centerPos.y, -1);
        Camera.main.orthographicSize = 15;
        Camera.main.GetComponent<CameraScript>().inFinalBoss = true;
    }

    public override void Exit()
    {
        boss.gun.canFire = false;
    }

    public override void Tick()
    {
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