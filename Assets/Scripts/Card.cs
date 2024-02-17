using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public static readonly int[] CARD_VALUE_RANGE_EXCLUSIVE = { 1, 13 };

    [SerializeField] CardsInHand hand;

    [Header("Fields for the Card")]
    [SerializeReference] private Sprite CardSprite;
    [SerializeReference] private Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;

    [Header("Attributes Used Mainly for HandDisplay")]
    [SerializeReference] public Vector2 previousPos;

    //Method to change the values; Called after instantiating the card; https://forum.unity.com/threads/instantiate-a-object-with-a-constructor.1315239/
    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
        this.name = cardType.name + " of " + cardValue;
    }
    public int GetCardValue() { return this.cardValue; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }


    private void OnDisable() { hand.Remove(this); }
    private void OnEnable() { hand.Add(this); }


    // Start is called before the first frame update
    void Start()
    {
        if (this.cardType == null)
        {
            throw new System.Exception("Instance of Card: " + this.GetType().Name + " did not initizated. Did you forget to run it through the Instantiation class method?"); 
        }
    }

}
