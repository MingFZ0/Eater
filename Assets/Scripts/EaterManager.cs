using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterManager : MonoBehaviour
{

    public static Card[] EaterList = new Card[2];
    public static int[] ValueLeftToEat = new int[2];
    int[] recordedValueLeftToEat = new int[2];
    int recordedEaterListCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (recordedEaterListCount != EaterList.Length)
        {
            recordedEaterListCount = EaterList.Length;
            UpdateEaterList();
        }

        if (TurnManager.PhaseCount == 0)
        {
            recordedValueLeftToEat = ValueLeftToEat;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(EaterManager.ValueLeftToEat[0]);
            Debug.Log(EaterManager.ValueLeftToEat[1]);
        }

    }

    private void UpdateEaterList()
    {
       //if (EaterList[0])
       //{
       //     ValueLeftEater1 = EaterList[0].CardValue;

       //} else { ValueLeftEater1 = 0; }
       
       //if (EaterList[1])
       //{
       //     ValueLeftEater2 = EaterList[1].CardValue;
       //}
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
            foreach (Card card in EaterManager.EaterList)
            {
                if (card.gameObject.GetInstanceID() == cardInstanceID)
                {
                    Debug.Log($"Card Found in EaterManager: {card}");
                    return card;
                }
            }
        }

        else if (hitObject.transform.gameObject.tag == "Card")
        {
            return null;
        }

        Debug.LogWarning($"{gameObject.name} Unable to Convert RaycastHit2D of {hitObject.transform.gameObject.name} to Object");
        return null;
    }
}
