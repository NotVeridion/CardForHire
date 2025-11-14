using UnityEngine;
using System.Collections.Generic;

public class DeckManagerScript : MonoBehaviour
{
    public float cardDuration;
    public Deck chosenDeck;
    public int numShuffles;
    public Card currentCard;
    private List<Card> storedDeck;
    private List<Card> workingDeck;
    private float currentTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initial decks and card
        storedDeck = chosenDeck.GetStartingDeck();
        workingDeck = FillWorkingDeck(); // Takes in # of shuffles
        currentCard = DrawCard();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= cardDuration)
        {
            // Draw new card after current card runs out
            // DrawCard() also refills working deck if empty
            currentCard = DrawCard();
            currentTime = 0;
        }
    }

    public Card getCurrentCard()
    {
        return currentCard;
    }
    
    Card DrawCard()
    {
        // Fill working deck if empty
        if (workingDeck.Count == 0)
        {
            workingDeck = FillWorkingDeck();
        }

        Card card = workingDeck[0];
        workingDeck.RemoveAt(0);

        Debug.Log("New card drawn in deck manager: " + card.number + " " + card.suit);

        return card;
    }

    List<Card> FillWorkingDeck()
    {
        List<Card> newDeck = new List<Card>(storedDeck);

        // Deck shuffle using Fisher-Yates algorithm repeated {numShuffles} times
        for (int i = 0; i < numShuffles; i++)
        {
            for(int j = newDeck.Count - 1; j > 1; j--)
            {
                int idx = Random.Range(j, newDeck.Count - 1);
                Card temp = newDeck[idx];
                newDeck[idx] = newDeck[j];
                newDeck[j] = temp;
            }
        }
        
        Debug.Log("Deck reshuffled!");
        return newDeck;
    }
}
