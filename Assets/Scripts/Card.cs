using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public static readonly int[] CARD_VALUE_RANGE_EXCLUSIVE = { 1, 13 }; 

    // Fields for the card
    [SerializeReference] private readonly Sprite CardSprite;
    [SerializeReference] private readonly Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;

    [SerializeField] CardsInHand hand;

    public Card(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
    }

    public int GetCardValue() { return this.cardValue; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }

    private void OnDisable() { hand.Remove(this); }
    private void OnEnable() { hand.Add(this); }




    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
