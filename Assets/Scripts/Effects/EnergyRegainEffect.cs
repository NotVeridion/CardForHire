using UnityEngine;

public class EnergyRegainEffect : TimedEffect
{
    [HideInInspector]
    public PlayerScript player;
    public float amt;

    protected override void ApplyEffect()
    {
        player.ChangeStat("EnergyRegain", -amt);
    }
}