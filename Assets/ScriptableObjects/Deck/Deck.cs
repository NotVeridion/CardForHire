using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Deck", menuName = "Scriptable Objects/Deck")]
public class Deck : ScriptableObject
{
    public List<Card> deck;
}
