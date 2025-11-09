using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Deck", menuName = "Scriptable Objects/Deck")]
public class Deck : ScriptableObject
{
    [SerializeField] private List<Card> deck;


    public List<Card> GetStartingDeck()
    {
        List<Card> result = new List<Card>();
        foreach(Card card in deck)
        {
            result.Add(Instantiate<Card>(card));
        }
        return result;
    }
}
