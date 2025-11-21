using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Scriptable Objects/Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public Sprite gunSprite;
    public float fireRate;
    public float damage;
    public float bulletMoveSpeed;
    public float bulletDuration;
    public bool isSingleShot;
    public bool isSpreadShot;
    public float spreadRange;
    public int numBulletsInSpread;
}
