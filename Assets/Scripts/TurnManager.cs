using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;
    public static TurnManager Instance { get { return _instance; } }


    public static int RoundCount = 1;
    public static int TurnCount = 0;
    public static int PhaseCount;
    public int TotalScore;

    [SerializeField] TMP_Text roundCounter;
    [SerializeField] TMP_Text turnCounter;
    [SerializeField] TMP_Text phaseCounter;

    [SerializeField] List<GameObject> eaterSlotList = new List<GameObject>();

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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PhaseCount == 2)
        {
            NextTurn();
        }

        if (TurnCount == 0 && PhaseCount == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
                //Debug.Log(mousePosInWorld);

                RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);

                if (hit.collider && hit.transform.gameObject.tag == "Deck")
                {
                    NextTurn();
                }
            }
        }

        phaseCounter.text = "Phase " + PhaseCount.ToString();
    }

    public void ResetPhase()
    {

        PhaseCount = 0;
    }

    private void NextTurn()
    {
        if (PrizeCardManager.PrizeCardsList.Count <= 0)
        {
            Debug.Log("New Round!");

            EaterManager.EaterList.Clear();
            PrizeCardManager.PrizeCardsList.Clear();
            PrizeCardManager.Instance.InitalizePrizePile();

            HandManager.CardsInHand.Clear();

        }

        foreach (Card card in EaterManager.EaterList)
        {
            if (card.EaterKilled == true)
            {
                card.GetComponent<SpriteRenderer>().color = card.FaceDownColor;
            }

            EaterDisable();
        }

        PhaseCount = 0;
        TurnCount++;
        DiscardPile.DiscardCount = 0;
        turnCounter.text = "Turn " + TurnCount.ToString();
    }

    void EaterDisable()
    {
        foreach (GameObject eaterSlot in eaterSlotList)
        {
            RaycastHit2D hit = Physics2D.Raycast(eaterSlot.transform.position, Vector2.zero);

            if (hit.collider && hit.transform.gameObject.tag == "Card")
            {
                Card card = HandManager.Instance.RaycastHit2DToObject(hit, "EaterList");
                if (card.EaterKilled) 
                {
                    card.gameObject.SetActive(false);
                    eaterSlot.gameObject.SetActive(false);
                }

            }
        }
    }
}
