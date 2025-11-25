using UnityEngine;

public class BleedEffect : TimedEffect
{
    public float bleedDmg;

    protected override void ApplyEffect()
    {
        target.TakeDamage(bleedDmg);
    }
}