using UnityEngine;

public class BossIdleState : BossState
{
    public BossIdleState(BossScript boss) : base(boss) {}

    public override void Enter()
    {
        boss.rb.linearVelocity = Vector2.zero;
    }

    public override void Exit()
    {
        
    }

    public override void Tick()
    {
        
    }
}