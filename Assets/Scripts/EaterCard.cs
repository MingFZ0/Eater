using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterCard : MonoBehaviour
{
    [SerializeField] EaterList eaterList;
    [SerializeField] CardsInHand hand;

    [Header("Fields for the EaterCard")]
    [SerializeReference] private Sprite CardSprite;
    [SerializeReference] private Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;
    [SerializeField] private TextMesh displayText;
    [SerializeReference] private bool isInstantiated;

    [Header("Fields that relates to Feeding")]
    [SerializeReference] private int hungerValue;
    [SerializeReference] private bool isFull;

    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
        this.name = "EATER " + cardType.name + " of " + cardValue;
        displayText.text = cardValue.ToString();
        this.isInstantiated = true;

        this.hungerValue = cardValue;
    }

    public int GetCardValue() { return this.cardValue; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }

    //These functions are UnityEvent Functions. You need to attach the GameEventListener script to this gameobject
    //and from there, select this functions to run when the specific UnityEvent is raised.
    public void TrySpawnEater()
    {
        if (this.isInstantiated) { return; }
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);

        if (hit.collider && hand.selectedCard != null)
        {
            if (hit.collider.gameObject != hand.selectedCard.gameObject) { return; }
            Card card = hand.selectedCard;
            Instantiation(card.GetCardValue(), card.GetCardType());
            hand.Destory(card);
        }
    }

    public void EndPhaseUpdate()
    {
    }


    private void OnDisable() { 
        if (eaterList.EaterCount == 1)
        {
            throw new System.Exception("GAME OVER!");
        }
        eaterList.Remove(this); 
    }
    private void OnEnable() { eaterList.Add(this); }


    private void Update()
    {
        if (!this.isInstantiated || this.isFull) { return; }

        if (Input.GetMouseButtonUp(0) && hand.selectedCard != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);
            if (hit.collider == null || hit.collider.gameObject != hand.selectedCard.gameObject) { return; }

            Card card = hand.selectedCard;
            if (card.GetCardValue() <= this.hungerValue)
            {
                this.hungerValue -= card.GetCardValue();
                Debug.Log("Gabble Gabble. Eater " + name + " has " + hungerValue + " hunger points left");
                Destroy(card.gameObject);
            }

            if (hungerValue == 0) { this.isFull = true; }
        }
    }

}
