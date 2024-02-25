using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
    [SerializeField] private Card emptyCard;
    [SerializeField] private CardsInHand hand;
    [SerializeField] private PrizeCardList prizeList;
    [SerializeField] private EaterList feedingList;
    [SerializeField] private List<CardTypeEnumScriptableObject> availableCardTypes = new List<CardTypeEnumScriptableObject>();

    [SerializeField] private GameVariables gameVars;
    [SerializeField] private UnityEvent UpdateHand;

#if UNITY_EDITOR
    private void Awake()
    {
        if (availableCardTypes.Count == 0) {Debug.LogWarning("There are no Available Card Type set assigned. Are you sure you want to proceed with such affecting the CreateCard Method? "); }
        else
        {
            //Get the path of the folder for the first item in the array
            string firstCardTypePath = AssetDatabase.GetAssetPath(availableCardTypes[0]);
            DirectoryInfo exampleParent = Directory.GetParent(firstCardTypePath);

            //Compares it to the rest of the items in the array
            foreach (CardTypeEnumScriptableObject cardType in availableCardTypes)
            {
                string parentPath = AssetDatabase.GetAssetPath(cardType);
                DirectoryInfo parent = Directory.GetParent(parentPath);

                if (!exampleParent.FullName.Equals(parent.FullName))
                {
                    Debug.LogWarning("The Scrptiable Object Enum" + cardType + " does not have matching directory with the rest of the cardType List");
                }
            }
        }
    }
    #endif

    private void _createCard()
    {
        int cardValue = Random.Range(gameVars.GetCARD_VALUE_RANGE()[0], gameVars.GetCARD_VALUE_RANGE()[1]);
        int cardTypeIndex = Random.Range(0, availableCardTypes.Count);
        CardTypeEnumScriptableObject cardType = availableCardTypes[cardTypeIndex];

        while (hand.ContainSameValueInList(cardValue, cardType))
        {
            cardValue = Random.Range(gameVars.GetCARD_VALUE_RANGE()[0], gameVars.GetCARD_VALUE_RANGE()[1]);
            cardTypeIndex = Random.Range(0, availableCardTypes.Count);
            cardType = availableCardTypes[cardTypeIndex];
        }

        Card card = Instantiate(emptyCard);
        card.Instantiation(cardValue, cardType);
        hand.UpdateHandDisplay();
    }

    public void CreateCard()
    {

        if (hand.GetHandLength() == 0 && feedingList.GetIsFeeding() == false) { 
            for (int i = 0; i < gameVars.GetHAND_SIZE(); i++)
            {
                _createCard();
            }
            if (gameVars.Turn == 0) { gameVars.MoveToNextPhase(); }

        } else if (gameVars.GetGamePhase().name == "DRAW_PHASE")
        {
            if (prizeList.revealed == null)
            {
                _createCard();
                gameVars.MoveToNextPhase();
            }
            else { throw new System.Exception("You didn't draw your prize card"); }
        }
    }


}
