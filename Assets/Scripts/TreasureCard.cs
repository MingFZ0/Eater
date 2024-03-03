using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCard : MonoBehaviour
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

    private void revealCard()
    {
        this.displayText.text = cardValue.ToString();
        this.cardValue = Random.Range(gameVar.GetCARD_VALUE_RANGE()[0], gameVar.GetCARD_VALUE_RANGE()[1]);
        int cardTypeIndex = Random.Range(0, gameVar.GetAvailableCardTypes().Count);
        CardTypeEnumScriptableObject cardType = gameVar.GetAvailableCardTypes()[cardTypeIndex];

        gameVar.EndRound();
        Card card = Instantiate(emptyCard);
        card.Instantiation(cardValue, cardType);
        hand.UpdateHandDisplay();
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && prizeList.PrizeCardsCount == 0)
        {
            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);

            if (!hit.collider) { return; }
            if (hit.collider.gameObject == this.gameObject)
            {
                if (gameVar.GetGamePhase().name == "ACTION_PHASE") { revealCard(); }
            }

        }

    }
}
