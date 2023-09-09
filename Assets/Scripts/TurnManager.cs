using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static int RoundCount = 1;
    public static int TurnCount = 0;
    public static int PhaseCount;
    public int TotalScore;

    [SerializeField] TMP_Text roundCounter;
    [SerializeField] TMP_Text turnCounter;

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

        if (TurnCount == 0)
        {
            if (EaterManager.EaterList[0] != null && EaterManager.EaterList[1] != null && Input.GetMouseButtonDown(0))
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
    }

    void InitializeTurn()
    {

    }

    public void ResetPhase()
    {
        PhaseCount = 0;
    }

    public void NextTurn()
    {

        PhaseCount = 0;
        TurnCount++;
        DiscardPile.DiscardCount = 0;
        Debug.Log(TurnCount);
        turnCounter.text = "Turn " + TurnCount.ToString();
    }
}
