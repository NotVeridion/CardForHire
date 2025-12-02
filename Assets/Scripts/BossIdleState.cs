using UnityEngine;

public class BossIdleState : BossState
{
    public BossIdleState(BossScript boss) : base(boss) {}

    public override void Enter()
    {
        Vector3 direction = (boss.positionStart.transform.position - boss.transform.position).normalized;
        boss.rb.linearVelocity = direction * boss.bossMoveSpeed;
        boss.gun.canFire = false;
        boss.currentHealth = boss.maxHealth;
    }

    public override void Exit()
    {
        boss.gun.canFire = true;
    }

    public override void Tick()
    {
        if (Vector3.Distance(boss.transform.position, boss.positionStart.transform.position) < 1)
        {
            boss.rb.linearVelocity = Vector3.zero;
        }
    }
}