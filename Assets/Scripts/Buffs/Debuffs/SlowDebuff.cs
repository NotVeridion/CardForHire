using UnityEngine;

public class SlowEffect : TimedEffect
{
    public float slowAmt;

    protected override void ApplyEffect()
    {
        target.ChangeStat("Slow", slowAmt);
    }

    protected override void EndEffect()
    {
        target.ChangeStat("Slow", -slowAmt);
        base.EndEffect();
    }
}