using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterManager : MonoBehaviour
{
    private static EaterManager _instance;
    public static EaterManager Instance { get { return _instance; } }

    public static List<Card> EaterList = new List<Card>();
    [SerializeField] public static List<Card> StoredList = new List<Card>();
    [SerializeField] public static List<Card> FullList = new List<Card>();

    private static Card _currentEater;
    public static Card CurrentEater
    {
        get { return _currentEater; }
        set
        {
            if (_currentEater == value)
            {
                if (FullList.Contains(value))
                {
                    _currentEater = null;
                    return;
                }
            }
            else if (!FullList.Contains(_currentEater))
            { 
                if (_currentEater != null) { _currentEater = null; }
                _currentEater = value;
                _currentHunger = _currentEater.CardValue;
                Debug.Log($"FullHunger is {CurrentHunger}");
            }
        }
    }

    private static int _currentHunger;
    public static int CurrentHunger 
    { 
        get { return _currentHunger; } 
        set
        {
            //Debug.Log($"{CurrentHunger} - {value} vs. {CurrentHunger - value}");
            _currentHunger = value;

            if ((_currentHunger) < 0)
            {
                Debug.Log("Too Full!");

                _currentHunger = CurrentEater.CardValue;

                foreach (Card card in StoredList)
                {
                    Debug.Log($"Spitting back out {card.name}");
                    card.gameObject.SetActive(true);
                    card.stored = false;

                }
                StoredList.Clear();
            }
            else if ((_currentHunger) == 0)
            {
                foreach (Card card in StoredList)
                {
                    card.stored = false;
                    Destroy(card.gameObject);
                }

                FullList.Add(CurrentEater);
                StoredList.Clear();
                Debug.Log($"{CurrentEater.name} is now full");
                _currentEater = null;

                if (FullList.Count == EaterList.Count)
                {
                    TurnManager.PhaseCount = 3;
                }

                //_currentHunger = value;
            }
            
        }
    }

    int[] recordedValueLeftToEat = new int[2];
    int recordedEaterListCount;

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

        if (recordedEaterListCount != EaterList.Count)
        {
            recordedEaterListCount = EaterList.Count;
        }

        if (TurnManager.PhaseCount == 0)
        {
            //recordedValueLeftToEat = ValueLeftToEat;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Current Hunger is {CurrentHunger}");
            Debug.Log($"Current StoreList size is {StoredList.Count}");
            Debug.Log($"Current FullList size is {FullList.Count}");
            Debug.Log($"Current Eater is {_currentEater.name}");
        }

        KillCheck();

    }

    void KillCheck()
    {
        if (TurnManager.TurnCount <= 0 || TurnManager.PhaseCount <= 0) { return; }
        {
            if (Input.GetMouseButtonDown(0))      //Detect card being moved to EaterSlot
            {
                Vector2 mousePosOnScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
                //Debug.Log(mousePosInWorld);

                RaycastHit2D hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero, 0f);

                Card selectedCard = RaycastHit2DToObject(hit, "EaterList");

                if (selectedCard == null) { return; }

                if (selectedCard.EaterKilled)
                {
                    selectedCard.EaterKilled = false;
                    Debug.Log($"{selectedCard.transform.name} is not being killed");
                    selectedCard.transform.GetComponentInChildren<TextMesh>().color = Color.black;
                    selectedCard.GetComponent<SpriteRenderer>().color = selectedCard.FaceUpColor;

                    //if (!EaterList.Contains(selectedCard))
                    //{
                    //    EaterList.Add(selectedCard);
                    //}    

                    
                }
                else if (!selectedCard.EaterKilled)
                {
                    selectedCard.EaterKilled = true;
                    Debug.Log($"{selectedCard.transform.name} is being killed off");
                    selectedCard.transform.GetComponentInChildren<TextMesh>().color = Color.red;
                    selectedCard.GetComponent<SpriteRenderer>().color = selectedCard.FaceDownColor;

                    if (StoredList.Count > 0)
                    {
                        foreach (Card card in StoredList)
                        {
                            card.stored = false;
                            card.gameObject.SetActive(true);
                        }
                    }

                    //_currentEater = null;
                    //_currentHunger = 0;
                    //EaterList.Remove(selectedCard);

                    //if (FullList.Contains(selectedCard)) { FullList.Remove(selectedCard); }
                    //if (FullList.Count == 1 || EaterList.Count == 0) { TurnManager.PhaseCount = 3; }

                }
            }
        }
    }

    private Card RaycastHit2DToObject(RaycastHit2D hitObject, string targetList)
    {
        if (!hitObject.collider || hitObject.transform.gameObject.tag != "Card") { return null; }

        int cardInstanceID = hitObject.transform.gameObject.GetInstanceID();

        //if (hitObject.transform.gameObject.tag == "Card" && hitObject.transform.gameObject != EaterSelected)
        //{
        //    Debug.Log("The eater for this slot has already been selected");
        //    return null;
        //}

        if (targetList == "HandList")
        {
            foreach (Card card in HandManager.CardsInHand)
            {
                if (card.gameObject.GetInstanceID() == cardInstanceID)
                {
                    Debug.Log($"Card Found in HandManager: {card}");
                    return card;
                }
            }
        }

        else if (targetList == "EaterList")
        {
            Card[] cardList = FindObjectsOfType<Card>();
            foreach (Card card in cardList)
            {
                if (card.gameObject.GetInstanceID() == cardInstanceID)
                {
                    if (card.Eater == false) { return null; }
                    Debug.Log($"Card Found Via cardType: {card}");
                    return card;
                }
            }
        }

        else if (hitObject.transform.gameObject.tag == "Card")
        {
            return null;
        }

        Debug.LogWarning($"{gameObject.name} Unable to Find {hitObject.transform.gameObject.name} within {targetList}");
        return null;
    }


}
