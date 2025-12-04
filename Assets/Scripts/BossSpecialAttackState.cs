using UnityEngine;
using System.Collections;

public class BossSpecialAttackState : BossState
{
    public BossSpecialAttackState(BossScript boss) : base(boss) {}

    private Vector3 chosenPos;
    private int currentPosIndex;
    private bool fired;
    public override void Enter()
    {

        chosenPos = GetRandomSpecialPosition();
        MoveTowardsPosition();

        boss.gun.currentGun = boss.gun.bossPistol;
    }

    public override void Exit()
    {
        boss.gun.canFire = false;
    }

    public override void Tick()
    {
        // Just arriving on position
        if (isOnPosition())
        {
            boss.rb.linearVelocity = Vector3.zero;
            if (boss.gun.canFire)
            {
                boss.gun.Shoot();
                chosenPos = GetRandomSpecialPosition();
                fired = true;
            }
        }
        // Wait at position while canFire is false
        else if (fired && !boss.gun.canFire)
        {
            boss.rb.linearVelocity = Vector3.zero;
        }
        // After waiting, move to next position
        else if (fired && boss.gun.canFire)
        {
            fired = false;
            MoveTowardsPosition();
        }

    }

    bool isOnPosition()
    {
        float distance = Vector3.Distance(boss.transform.position, chosenPos);

        if (distance < 2)
        {
            return true;
        }

        return false;
    }

    Vector3 GetRandomSpecialPosition()
    {
        // Keep getting new index
        int randIndex = Random.Range(0, boss.positionObjects.Length);
        while (randIndex == currentPosIndex){
            randIndex = Random.Range(0, boss.positionObjects.Length);
        }

        currentPosIndex = randIndex;

        GameObject obj = boss.positionObjects[randIndex];
        Vector3 pos = obj.transform.position;
        
        return pos;
    }

    void MoveTowardsPosition()
    {
        Vector3 direction = (chosenPos - boss.transform.position).normalized;
        boss.rb.linearVelocity = direction * boss.bossMoveSpeed * 4;
    }
}