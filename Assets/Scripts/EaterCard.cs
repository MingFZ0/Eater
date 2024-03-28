using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EaterCard : MonoBehaviour
{
    [SerializeField] GameVariables gameVars;
    [SerializeField] CardsInUse cardsInUse;
    [SerializeField] EaterList eaterList;
    [SerializeField] CardsInHand hand;

    [Header("Fields for the EaterCard")]
    private SpriteRenderer spriteRenderer;

    private Sprite cardSprite;
    [SerializeReference] private Sprite cardBackSprite;
    [SerializeField] private SpriteRenderer cardBackgroundRenderer;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;
    [SerializeField] private TextMesh displayText;
    [SerializeReference] private bool isInstantiated;
    [SerializeReference] private int totalCardScore;

    [Header("Fields that relates to Feeding")]
    [SerializeReference] private int hungerValue;
    [SerializeReference] private bool isFull;
    [SerializeField] private UnityEvent eaterFed;
    [SerializeReference] private List<Card> feedingList;
    [SerializeField] private UnityEvent scoreDisplay;

    private void Start()
    {
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardBackSprite;
    }

    // === Getters or Setters === //

    public int GetCardValue() { return this.cardValue; }
    public int GetTotalCardScore() { return totalCardScore; }
    public int GetHungerValue() { return hungerValue; }
    public bool GetIsFull() { return isFull; }
    public CardTypeEnumScriptableObject GetCardType() { return this.cardType; }

    // === Methods Related to Spawning === //

    /// <summary>
    /// An response method that triggers from the eaterFed event. need to attach the GameEventListener script to this gameobject
    ///and from there, select this functions to run when the specific UnityEvent is raised
    /// </summary>
    public void TrySpawnEater()
    {
        if (this.isInstantiated) { return; }
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);

        if (hit.collider && hand.selectedCard != null)
        {
            if (hit.collider.gameObject != hand.selectedCard.gameObject) { return; }
            if (hand.selectedCard.GetCardType().getCardType() == "JOKER") { return; }
            Card card = hand.selectedCard;
            Instantiation(card.GetCardValue(), card.GetCardType());
            hand.Destory(card);
        }
    }

    /// <summary>
    /// Assign the eater card with the coorsponding card Value and card Type
    /// </summary>
    /// <param name="cardValue"></param>
    /// <param name="cardType"></param>
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

    // === Methods Related to Feeding === //

    /// <Summary> 
    /// This method is used for feeding. It also checks if the 
    /// current eater is full and does the corresponding updates
    /// </Summary>
    private void feeding()
    {
        if (gameVars.Turn == 0) { return; }
        Card card = hand.selectedCard;
        if (card.GetCardValue() <= this.hungerValue || (card.GetCardType().getCardType() == "JOKER" && this.hungerValue > 0))
        {
            if (card.GetCardType().getCardType() == "JOKER") { this.hungerValue = 0; }
            else { this.hungerValue -= card.GetCardValue(); }
            displayText.text = hungerValue.ToString();
            if (hungerValue > 0) {spriteRenderer.sprite = cardsInUse.getCardSprite(cardType, hungerValue); }


            card.gameObject.SetActive(false);
            hand.UpdateHandDisplay();

            gameVars.AddScore();
            totalCardScore++;

            feedingList.Add(card);
            eaterList.FeedingUpdate();
            eaterFed.Invoke();      //invokes the eaterFed event which triggers phaseDisplay to update any stats needed

        }

        if (this.hungerValue == 0)
        { 
            clearCardsFed();
            eaterList.FeedingUpdate();
            this.isFull = true;
            eaterList.IncrementEaterFull();
            spriteRenderer.sprite = cardsInUse.getCardSprite(cardType, cardValue);
            cardBackgroundRenderer.color = Color.grey;

        }

    }

    /// <summary>
    /// Spits out all cards in the feedingList
    /// </summary>
    public void spit()
    {
        foreach (Card card in feedingList)
        {
            card.gameObject.SetActive(true);
            hand.UpdateHandDisplay();
            totalCardScore--;
            gameVars.SubtractScore();
        }

        feedingList.Clear();
        scoreDisplay.Invoke();
        
        this.hungerValue = cardValue;
        eaterList.FeedingUpdate();
        displayText.text = this.hungerValue.ToString();
        spriteRenderer.sprite = cardSprite;
    }

    /// <summary>
    /// Clears the feedingList
    /// </summary>
    private void clearCardsFed()
    {
        foreach (Card card in feedingList)
        {
            cardsInUse.addToUnavailable(card);
            Destroy(card.gameObject);
        }

        feedingList.Clear();
    }

    // === Methods Related to data reset between turns/rounds === //

    /// <summary>
    /// Updates at the start of the turn
    /// </summary>
    public void StartOfTurnUpdate()
    {
        this.isFull = false;
        this.hungerValue = this.cardValue;
        this.feedingList.Clear();
        displayText.text = this.hungerValue.ToString();
        cardBackgroundRenderer.color = Color.white;
    }

    /// <summary>
    /// Reset current data to setup for the next round
    /// </summary>
    public void NextRoundSetups()
    {
        this.cardValue = 0;
        this.cardType = null;
        this.name = "Uninstantiated";
        this.totalCardScore = 0;
        displayText.text = name;

        this.isInstantiated = false;
        this.hungerValue = 0;
        this.spriteRenderer.sprite = cardBackSprite;
    }
    private void OnValidate()
    {
        feedingList.Clear();
    }

    private void OnDisable() { eaterList.SubtractNumOfEaterAlive(); }
    private void OnEnable() 
    { 
        eaterList.Add(this);
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
            if (hit.collider == null || hit.collider.gameObject != this.gameObject || this.feedingList.Count == 0) { return; }
            spit();
        }
    }

}
