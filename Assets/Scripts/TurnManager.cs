using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] Button saveButon;

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
        if (PhaseCount == 4)
        {
            NextTurn();
        }

        if (TurnCount == 0 && PhaseCount == 2)
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

        NewRound();
        
        phaseCounter.text = "Phase " + PhaseCount.ToString();
        turnCounter.text = "Turn " + TurnCount.ToString();
        roundCounter.text = "Round " + RoundCount.ToString();
    }

    public void ResetPhase()
    {

        PhaseCount = 0;
    }
    void PhaseRun()
    {
        //Phase 0 is save or not save

        //Phase 1 is drawing deck

        //Phase 2 is drawing prize card

        //Phase 3 is feeding and discard

        //Phase 4 is flipping over prize card
    }

    private void NextTurn()
    {

        foreach (Card card in EaterManager.EaterList)
        {
            if (card.EaterKilled == true)
            {
                card.GetComponent<SpriteRenderer>().color = card.FaceDownColor;
            }

            EaterDisable();
        }

        if (EaterManager.EaterList.Count == 0 && TurnCount > 0)
        {
            Debug.Log("GGs, You Lost");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        PhaseCount = 0;
        TurnCount++;
        DiscardPile.DiscardCount = 0;
        EaterManager.FullList.Clear();
    }

    public void SaveRound()
    {
        print("Round is saved");
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

    void NewRound()
    {
        if (!(PrizeCardManager.PrizeCardsList.Count == 0 && TurnCount > 3 && EaterManager.EaterList.Count == EaterManager.FullList.Count)) { return; }

        Debug.Log("New Round!");

        EaterManager.EaterList.Clear();
        HandManager.CardsInHand.Clear();

        Card[] cardList = FindObjectsOfType<Card>();
        foreach (Card card in cardList)
        {
            Destroy(card.gameObject);
        }

        //set card slots active

        //spawn in the prizecards
        PrizeCardManager.Instance.InitalizePrizePile();

        PhaseCount = 0;
        TurnCount = 0;
        RoundCount++;

    }
}
