using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeckManagerScript : MonoBehaviour
{
    public float cardDuration;
    public Deck chosenDeck;
    public int numShuffles;
    public Card currentCard;
    public List<Card> storedDeck;
    public Slider cardDurationSlider;
    private List<Card> workingDeck;
    private float currentTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartDeck();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        cardDurationSlider.value = 1 - (currentTime / cardDuration);

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

    public List<Card> FillWorkingDeck()
    {
        List<Card> newDeck = new List<Card>(storedDeck);

        // Deck shuffle using Fisher-Yates algorithm repeated {numShuffles} times
        for (int i = 0; i < numShuffles; i++)
        {
            for(int j = newDeck.Count - 1; j >= 0; j--)
            {
                int idx = Random.Range(j, newDeck.Count);
                Card temp = newDeck[idx];
                newDeck[idx] = newDeck[j];
                newDeck[j] = temp;
            }
        }
        
        Debug.Log("Deck reshuffled!");
        return newDeck;
    }

    public void StartDeck()
    {
        // Initial decks and card
        storedDeck = chosenDeck.GetStartingDeck();
        workingDeck = FillWorkingDeck();
        currentCard = DrawCard();
    }
}
