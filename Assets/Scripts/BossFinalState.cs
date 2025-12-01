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
        boss.gun.currentGun = boss.gun.bossShotgun;
        bossGun = GameObject.FindWithTag("BossGun");
        boss.gun.canFire = true;
        boss.gun.inFinalState = true;
        boss.gun.currentGun.fireRate += 3;
    }

    public override void Exit()
    {
        boss.gun.canFire = false;
    }

    public override void Tick()
    {
        // Don't shoot until centered
        if (Vector3.Distance(boss.positionCenter.transform.position, boss.transform.position) < 1)
        {
            boss.rb.linearVelocity = Vector3.zero;
            centered = true;
        }

        if (centered)
        {   
            bossGun.transform.eulerAngles = new Vector3(0f, 0f, bossGun.transform.eulerAngles.z+2f);
            boss.gun.Shoot();
        }
    }
}