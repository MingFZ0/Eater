using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeCard : MonoBehaviour
{
    [SerializeField] private GameVariables gameVar;

    [Header("Fields for the Card")]
    [SerializeField] CardsInHand hand;
    [SerializeField] private Card emptyCard;

    [Header("Fields for the PrizeCard")]
    [SerializeField] PrizeCardList prizeList;
    [SerializeReference] private Sprite CardSprite;
    [SerializeReference] private Sprite CardBackSprite;
    [SerializeReference] private int cardValue;
    [SerializeReference] private CardTypeEnumScriptableObject cardType;
    [SerializeReference] private TextMesh displayText;

    /* === [Fields for GameEvents For PrizeCards] */
    [Header("Fields for GameEvents For PrizeCards")]
    [SerializeField] private GameEvent PrizeCardClicked;          //When you click on a facedown prizecard, you reveal it
                                                                  //When you click on a revealed prizecard, you draw it


    private void OnDisable() { prizeList.Remove(this); }
    private void OnEnable() { prizeList.Add(this); }

    public void Instantiation(int cardValue, CardTypeEnumScriptableObject cardType)
    {
        this.cardValue = cardValue;
        this.cardType = cardType;
        name = "[PrizeCard] " + cardValue + " of " + cardType.ToString();

    }

    private void revealCard()
    {
        prizeList.revealed = this;
        this.displayText.text = cardValue.ToString();
        gameVar.MoveToNextPhase();
    }

    private void drawCard()
    {
        Card card = Instantiate(emptyCard);
        card.Instantiation(cardValue, cardType);
        hand.UpdateHandDisplay();
        prizeList.revealed = null;
        prizeList.Remove(this);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);

            if (!hit.collider) { return; }
            if (hit.collider.gameObject == this.gameObject)
            {
                if (prizeList.revealed == null && gameVar.GetGamePhase().name == "ACTION_PHASE") {revealCard();} 
                else if (prizeList.revealed == this && gameVar.GetGamePhase().name == "DRAW_PHASE") {drawCard();}
            }

        }

    }


}
