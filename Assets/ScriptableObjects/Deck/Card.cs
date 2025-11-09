using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public enum Suit
    {
        Hearts, //Heal from damage
        Diamonds, //Chance to gain some gold when hitting enemies
        Clubs, //Reduce Damage of enemies
        Spades //Larger Bullets
    }

    public Sprite sprite;
    public Suit suit;
    public int number;
    public bool slow; //Slow enemies
    public bool bleed; //Puts a bleed on enemies
    public bool pierce; //Shots can go through multiple enemies
    public bool knockOut; //Enemies are knocked out for a few seconds
    public bool energyRegain; //Reduce time of dash
    public bool faster; //Bullets are faster

}

