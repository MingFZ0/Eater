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

    [Header("Attributes Used for Card Display")]
    [SerializeReference] private bool isSelected;
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

    /* =================================================
        Section for Dragging and displaying the card
    */
    private void isSelectedCheck()
    {
        if (Input.GetMouseButtonDown(0) && hand.selectedCard == null)
        {
            Debug.Log("First Check");
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);
            if (hit.collider && (hit.collider.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()))
            {
                hand.selectedCard = this;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (this.cardType == null)
        {
            throw new System.Exception("Instance of Card: " + this.GetType().Name + " did not initizated. Did you forget to run it through the Instantiation class method?"); 
        }
    }

    private void Update()
    {

        isSelectedCheck();

        if (hand.selectedCard != null && hand.selectedCard != this) { return; }

        if (Input.GetMouseButton(0) && hand.selectedCard == this)
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            this.transform.position = mousePosInWorld;
            Debug.Log("Mouse is being held");
        }

        if (Input.GetMouseButtonUp(0) && hand.selectedCard == this)
        {
            hand.selectedCard = null;
            hand.UpdateHandDisplay();
        }




    }



}
