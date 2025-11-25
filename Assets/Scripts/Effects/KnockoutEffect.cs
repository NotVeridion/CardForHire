using UnityEngine;

public class KnockoutEffect : TimedEffect
{
    public float probability;
    protected override void ApplyEffect()
    {
        target.isStunned = true;
    }

    protected override void EndEffect()
    {
        target.isStunned = false;
        base.EndEffect();
    }
}