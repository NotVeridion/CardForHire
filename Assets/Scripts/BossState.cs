public abstract class BossState
{
    protected BossScript boss;
    public BossState(BossScript boss)
    {
        this.boss = boss;
    }

    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}