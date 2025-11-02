using UnityEngine;

[CreateAssetMenu(fileName = "GunScriptableObject", menuName = "ScriptableObjects/GunScriptableObject")]
public class GunScriptableObject : ScriptableObject
{
    public string gunName;
    public float fireRate;
    public float bulletMoveSpeed;
    public float bulletDamage;
    public float bulletDuration;
}
