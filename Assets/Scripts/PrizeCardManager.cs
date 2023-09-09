using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeCardManager : MonoBehaviour
{
    private static PrizeCardManager _instance;
    public static PrizeCardManager Instance { get { return _instance; } }


    // Start is called before the first frame update
    public static List<PrizeCard> PrizeCardsList = new List<PrizeCard>();
    [SerializeField] GameObject prizeSlot;
    [SerializeField] PrizeCard prizeCard;

    [SerializeField] float distanceBetweenEachCardStacked;

    [SerializeField] Card cardPrefab;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        InitalizePrizePile();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && TurnManager.TurnCount > 0)
        {

            Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
            //Debug.Log(mousePosInWorld);

            RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);

            if (hit.collider && hit.transform.gameObject.tag == "PrizeCard")
            {
                PrizeCard hitPrizeCard = RaycastHit2DToObject(hit);

                if (hitPrizeCard.IsFaceUp)
                {
                    ConvertPrizeCardToHandCard(hitPrizeCard);
                }
                else if (!hitPrizeCard.IsFaceUp)
                {
                    hitPrizeCard.IsFaceUp = true;
                    TurnManager.PhaseCount = 2;
                }
            }
        }
    }

    private void InitalizePrizePile()
    {
        int totalPrizeCards = TurnManager.RoundCount + 2;
        float newY = prizeSlot.transform.position.y;
        float newZ = prizeSlot.transform.position.z + (totalPrizeCards);

        for (int i = 0; i < totalPrizeCards; i++)
        {
            PrizeCard newPrizeCard = Instantiate(prizeCard);
            Debug.Log(prizeCard.gameObject.name);

            SpriteRenderer cardRenderer = newPrizeCard.GetComponent<SpriteRenderer>();
            float cardSize = cardRenderer.size.y;
            Vector3 spawnLocation = new Vector3(prizeSlot.transform.position.x, newY - cardSize, newZ);

            newPrizeCard.transform.position = spawnLocation;
            PrizeCardsList.Add(newPrizeCard);
            Debug.Log(spawnLocation.y);
            newY -= distanceBetweenEachCardStacked;
            newZ--;
        }
    }

    private void ConvertPrizeCardToHandCard(PrizeCard prizeCard)
    {
        Card newCard = Instantiate(cardPrefab);
        newCard.name = prizeCard.name;
        newCard.CardValue = prizeCard.CardValue;
        newCard.CardType = prizeCard.CardType;
        HandManager.CardsInHand.Add(newCard);
        Destroy(prizeCard.gameObject);
    }

    PrizeCard RaycastHit2DToObject(RaycastHit2D hitObject)
    {
        if (!hitObject.collider || hitObject.transform.gameObject.tag != "PrizeCard") { return null; }

        int cardInstanceID = hitObject.transform.gameObject.GetInstanceID();
        Debug.Log("Hit!");

        //if (hitObject.transform.gameObject.tag == "Card" && hitObject.transform.gameObject != EaterSelected)
        //{
        //    Debug.Log("The eater for this slot has already been selected");
        //    return null;
        //}

        foreach (PrizeCard prizeCard in PrizeCardsList)
        {
            if (prizeCard.gameObject.GetInstanceID() == cardInstanceID)
            {
                Debug.Log($"Card Found in HandManager: {prizeCard}");
                return prizeCard;
            }
        }

        Debug.LogWarning($"{gameObject.name} Unable to Convert RaycastHit2D of {hitObject.transform.gameObject.name} to Object");
        return null;
    }
}
