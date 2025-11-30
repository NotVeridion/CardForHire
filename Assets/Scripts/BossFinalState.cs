using UnityEngine;

public class BossFinalState : BossState
{
    public BossFinalState(BossScript boss) : base(boss) {}

    public override void Enter()
    {
        Vector2 direction = (boss.positionCenter.transform.position - boss.transform.position).normalized;
        boss.rb.linearVelocity = direction * boss.bossMoveSpeed * 2;
    }

    public override void Exit()
    {
    }

    public override void Tick()
    {
        if (Vector3.Distance(boss.positionCenter.transform.position, boss.transform.position) < 2)
        {
            boss.rb.linearVelocity = Vector3.zero;
        }
    }
}