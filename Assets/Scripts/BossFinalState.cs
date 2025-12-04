using UnityEngine;

public class BossFinalState : BossState
{
    public BossFinalState(BossScript boss) : base(boss) {}

    private GameObject bossGun;
    private bool centered;
    public override void Enter()
    {
        Vector2 direction = (boss.positionCenter.transform.position - boss.transform.position).normalized;
        boss.rb.linearVelocity = direction * boss.bossMoveSpeed * 2;
        boss.gun.currentGun = boss.gun.bossFinalGun;
        bossGun = GameObject.FindWithTag("BossGun");
        boss.gun.canFire = true;
    }

    public override void Exit()
    {
        boss.gun.canFire = false;
    }

    public override void Tick()
    {
        // Don't shoot until centered
        if (Vector3.Distance(boss.positionCenter.transform.position, boss.transform.position) < 2)
        {
            boss.rb.linearVelocity = Vector3.zero;
            centered = true;
        }

        if (centered)
        {   
            // Spin
            bossGun.transform.eulerAngles = new Vector3(0f, 0f, bossGun.transform.eulerAngles.z + (boss.finalSpinSpeed * Time.deltaTime));
            boss.gun.Shoot();
        }
    }
}