using UnityEngine;

public class SlowEffect : TimedEffect
{
    public float amt;

    protected override void ApplyEffect()
    {
        target.ChangeStat("Slow", -amt);
    }

    protected override void EndEffect()
    {
        target.ChangeStat("Slow", amt);
        base.EndEffect();
    }
}