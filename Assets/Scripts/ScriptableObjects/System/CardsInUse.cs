using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private UnityEvent resetEaten;

    [SerializeField] private GameVariables gameVar;


    private void OnValidate()
    {
        resetCards();

    }

    public void resetCards()
    {
        unavailableCards.Clear();
        availableCards.Clear();
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

    public void refill()
    {
        foreach (string cardName in unavailableCards)
        {
            availableCards.Add(cardName);
        }
        unavailableCards.Clear();
        resetEaten.Invoke();

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
        //Debug.Log(cardType.getCardType() + "_" + cardValue);
        if (cardType.getCardType() == "SPADE") { return spadeSet[cardValue-1]; }
        if (cardType.getCardType() == "CLOVER") { return cloverSet[cardValue-1]; }
        if (cardType.getCardType() == "DIAMOND") { return diamondSet[cardValue-1]; }
        if (cardType.getCardType() == "HEART") { return heartSet[cardValue-1]; }
        if (cardType.getCardType() == "JOKER") { return joker; }
        else { throw new System.Exception("Unknown card type"); }
    }

    public Sprite getCardSprite(string cardName)
    {
        string[] elements = cardName.Split("_");
        CardTypeEnumScriptableObject cardType = CardTypeEnumScriptableObject.stringToCardType(elements[0], gameVar.GetAvailableCardTypes());
        int cardValue = int.Parse(elements[1]);
        //Debug.Log(cardType.getCardType() + "_" + cardValue);
        if (cardType.getCardType() == "SPADE") { return spadeSet[cardValue - 1]; }
        if (cardType.getCardType() == "CLOVER") { return cloverSet[cardValue - 1]; }
        if (cardType.getCardType() == "DIAMOND") { return diamondSet[cardValue - 1]; }
        if (cardType.getCardType() == "HEART") { return heartSet[cardValue - 1]; }
        if (cardType.getCardType() == "JOKER") { return joker; }
        else { throw new System.Exception("Unknown card type"); }
    }
}
