using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    /* === [Fields for the Card] */
    [Header("Fields for the Card")]
    [SerializeField] CardsInHand hand;
    [SerializeReference] private Sprite CardSprite;
    [SerializeReference] private Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;
    [SerializeField] private TextMesh displayText;

    /* === [Fields for Card Display] */
    [Header("Attributes Used for Card Display")]
    [SerializeReference] private bool isSelected;
    [SerializeReference] public Vector2 previousPos;
    [SerializeReference] private CardsInUse cardsInUse;

    /* === [Fields for GameEvents For Dropped Cards] */
    [Header("Fields for GameEvents For Dropped Cards")]
    [SerializeField] private GameEvent trySpawnEater;


    /* Method to change the values; Called after instantiating the card; https://forum.unity.com/threads/instantiate-a-object-with-a-constructor.1315239/ */
    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
        name = cardType.name + " of " + cardValue;
        this.displayText.text = cardValue.ToString();
        
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardsInUse.getCardSprite(cardType, cardValue);
    }


    public int GetCardValue() { return this.cardValue; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }


    private void OnDisable() { 
        if (hand.selectedCard == this) { hand.selectedCard = null; }
        hand.Remove(this);
    }
    private void OnEnable() { hand.Add(this); }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
        hand.UpdateHandDisplay();
    }

    private void isSelectedCheck()
    {
        if (Input.GetMouseButtonDown(0) && hand.selectedCard == null)
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);
            if (hit.collider && (hit.collider.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()))
            {
                hand.selectedCard = this;
            }
        }
    }

    private void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && hand.selectedCard == this)
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            this.transform.position = mousePosInWorld;
        }
    }

    private void cardDropped()
    {
        trySpawnEater.Raise();
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

        OnMouseDrag();

        if (Input.GetMouseButtonUp(0) && hand.selectedCard == this)
        {
            cardDropped();
            hand.selectedCard = null;
            hand.UpdateHandDisplay();
        }

    }



}
