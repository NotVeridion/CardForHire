using UnityEngine;

public class FireRateEffect : TimedEffect
{
    [HideInInspector]
    public PlayerScript player;
    public float amt;

    protected override void ApplyEffect()
    {
        player.ChangeStat("FireRate", amt);
    }
    protected override void EndEffect()
    {
        player.ChangeStat("FireRate", -amt);
        base.EndEffect();
    }
}