using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsInUse", menuName = "ScriptableObjects/CardsInUse")]
public class CardsInUse : ScriptableObject
{
    [SerializeField] private Sprite[] spadeSet;
    [SerializeField] private Sprite[] cloverSet;
    [SerializeField] private Sprite[] diamondSet;
    [SerializeField] private Sprite[] heartSet;
    [SerializeField] private Sprite joker;

    [SerializeField] private List<string> availableCards;
    [SerializeField] private List<string> unavailableCards;     //This is for the cards that have been discarded or fed;
    [SerializeField] private List<CardTypeEnumScriptableObject> availableCardTypes = new List<CardTypeEnumScriptableObject>();
    [SerializeField] private Card emptyCard;
    [SerializeField] private PrizeCard emptyPrizeCard;

    [SerializeField] private GameVariables gameVar;


    private void OnValidate()
    {
        availableCards = new List<string>();
        foreach (CardTypeEnumScriptableObject cardType in gameVar.GetAvailableCardTypes())
        {
            //Add in all the rank and numbered cards
            for (int i = 1; i <= 13; i++)
            {
                string cardName = cardType.getCardType() + "_" + i;
                 //Debug.Log(cardName + " has been added to availableCards");
                availableCards.Add(cardName);
            }
        }

        //Add in the Jokers
        for (int i = 1; i <= 2; i++)
        {
            string cardName = "JOKER" + "_" + i;
            //Debug.Log(cardName + " has been added to availableCards");
            availableCards.Add(cardName);
        }

    }

    private void OnDisable()
    {
        //Debug.Log("Available cards has been cleared");
        availableCards.Clear();
    }

    public void addToUnavailable(Card card)
    {
        string cardName = card.GetCardType().getCardType() + "_" + card.GetCardValue();
        unavailableCards.Add(cardName);
    }

    private void refill()
    {
        foreach (string cardName in unavailableCards)
        {
            availableCards.Add(cardName);
        }
        unavailableCards.Clear();
    }

    public Card CreateCard()
    {
        if (availableCards.Count == 0) { refill(); }
        int cardNameIndex = Random.Range(0, availableCards.Count);
        string[] cardName = availableCards[cardNameIndex].Split("_");
        CardTypeEnumScriptableObject cardType = CardTypeEnumScriptableObject.stringToCardType(cardName[0], gameVar.GetAvailableCardTypes());
        Card card = Instantiate(emptyCard);
        card.Instantiation(int.Parse(cardName[1]), cardType);
        
        availableCards.Remove(availableCards[cardNameIndex]);
        return card;
    }

    public PrizeCard CreatePrizeCard(Vector3 spawnPos)
    {
        if (availableCards.Count == 0) { refill(); }
        int cardNameIndex = Random.Range(0, availableCards.Count);
        string[] cardName = availableCards[cardNameIndex].Split("_");
        CardTypeEnumScriptableObject cardType = CardTypeEnumScriptableObject.stringToCardType(cardName[0], gameVar.GetAvailableCardTypes());
        PrizeCard prizeCard = Instantiate(emptyPrizeCard, spawnPos, new Quaternion());
        prizeCard.Instantiation(int.Parse(cardName[1]), cardType);
       
        availableCards.Remove(availableCards[cardNameIndex]);
        return prizeCard;
    }

    public Sprite getCardSprite(CardTypeEnumScriptableObject cardType, int cardValue)
    {
        if (cardType.getCardType() == "SPADE") { return spadeSet[cardValue-1]; }
        if (cardType.getCardType() == "CLOVER") { return cloverSet[cardValue-1]; }
        if (cardType.getCardType() == "DIAMOND") { return diamondSet[cardValue-1]; }
        if (cardType.getCardType() == "HEART") { return heartSet[cardValue-1]; }
        if (cardType.getCardType() == "JOKER") { return joker; }
        else { throw new System.Exception("Unknown card type"); }
    }
}
