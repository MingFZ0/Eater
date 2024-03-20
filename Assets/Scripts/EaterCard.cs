using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EaterCard : MonoBehaviour
{
    [SerializeField] GameVariables gameVars;
    [SerializeField] CardsInUse cardsInUse;
    [SerializeField] EaterList eaterList;
    [SerializeField] EaterList feedingList;
    [SerializeField] CardsInHand hand;

    [Header("Fields for the EaterCard")]
    private SpriteRenderer spriteRenderer;

    private Sprite cardSprite;
    [SerializeReference] private Sprite cardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;
    [SerializeField] private TextMesh displayText;
    [SerializeReference] private bool isInstantiated;
    [SerializeReference] private int totalCardScore;

    [Header("Fields that relates to Feeding")]
    [SerializeReference] private int hungerValue;
    [SerializeReference] private bool isFull;
    [SerializeField] private UnityEvent eaterFed;
    [SerializeReference] private List<Card> cardsFed;
    [SerializeField] private UnityEvent scoreDisplay;

    private void Start()
    {
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = cardBackSprite;
    }

    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
        this.name = "EATER " + cardType.getCardType() + " of " + cardValue;
        displayText.text = cardValue.ToString();
        this.isInstantiated = true;

        this.hungerValue = cardValue;
        this.cardSprite = cardsInUse.getCardSprite(cardType, cardValue);
        this.spriteRenderer.sprite = cardSprite;
        eaterList.NotifyInstantiated();
        
    }
    public int GetCardValue() { return this.cardValue; }
    public int GetTotalCardScore() { return totalCardScore; }
    public int GetHungerValue() { return hungerValue; }
    public bool GetIsFull() { return isFull; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }


    /*These functions are UnityEvent Functions. You need to attach the GameEventListener script to this gameobject
               and from there, select this functions to run when the specific UnityEvent is raised.*/
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
    public void StartOfTurnUpdate()
    {
        this.isFull = false;
        this.hungerValue = this.cardValue;
        displayText.text = this.hungerValue.ToString();
    }

    /// <Summary> 
    /// This method is used for feeding. It also checks if the 
    /// current eater is full and does the corresponding updates
    /// </Summary>
    private void feeding()
    {
        Card card = hand.selectedCard;
        if (card.GetCardValue() <= this.hungerValue)
        {
            this.hungerValue -= card.GetCardValue();
            displayText.text = hungerValue.ToString();
            if (hungerValue > 0) {spriteRenderer.sprite = cardsInUse.getCardSprite(cardType, hungerValue); }


            card.gameObject.SetActive(false);
            hand.UpdateHandDisplay();
            feedingList.FeedingUpdate();

            gameVars.AddScore();
            totalCardScore++;

            eaterFed.Invoke();      //Mainly used for stopping player from feeding halfway to clear their hand and then draw more cards
            cardsFed.Add(card);

        }

        if (this.hungerValue == 0)
        { 
            feedingList.Remove(this);
            clearCardsFed();
            this.isFull = true;
            feedingList.FeedingUpdate();
            spriteRenderer.sprite = cardsInUse.getCardSprite(cardType, cardValue);

        }

    }
    public void spit()
    {
        foreach (Card card in cardsFed)
        {
            card.gameObject.SetActive(true);
            hand.UpdateHandDisplay();
            totalCardScore--;
            gameVars.SubtractScore();
        }

        cardsFed.Clear();
        scoreDisplay.Invoke();
        
        this.hungerValue = cardValue;
        displayText.text = this.hungerValue.ToString();
        spriteRenderer.sprite = cardSprite;
    }
    private void clearCardsFed()
    {
        foreach (Card card in cardsFed)
        {
            Destroy(card.gameObject);
        }

        cardsFed.Clear();
    }


    public void NextRoundSetups()
    {
        this.cardValue = 0;
        this.cardType = null;
        this.name = "Uninstantiated";
        displayText.text = name;

        this.isInstantiated = false;
        this.hungerValue = 0;
    }

    private void OnDisable() { eaterList.Remove(this); }
    private void OnEnable() 
    { 
        eaterList.Add(this);
        gameVars.AddToEaterRecord(this);
    }


    private void Update()
    {
        if (!this.isInstantiated) { return; }

        if (Input.GetMouseButtonUp(0) && hand.selectedCard != null && !this.isFull)
        {
            if (gameVars.GetGamePhase().name != "ACTION_PHASE") {return; }

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);
            if (hit.collider == null || hit.collider.gameObject != hand.selectedCard.gameObject) { return; }
            feeding();
        }

        else if (Input.GetMouseButtonDown(0) && hand.selectedCard == null && !this.isFull)
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero);
            if (hit.collider == null || hit.collider.gameObject != this.gameObject || this.cardsFed.Count == 0) { return; }
            spit();
        }
    }

}
