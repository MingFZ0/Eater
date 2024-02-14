using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    // Fields for the card
    private readonly Sprite CardSprite;
    private readonly Sprite CardBackSprite;
    private readonly int cardValue;
    private readonly CardTypeEnumScriptableObject cardType;

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
