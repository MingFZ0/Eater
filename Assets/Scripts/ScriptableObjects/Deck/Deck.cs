using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Deck : MonoBehaviour
{
    [SerializeField] private Card emptyCard;
    [SerializeField] private CardsInHand hand;
    [SerializeField] private List<CardTypeEnumScriptableObject> availableCardTypes = new List<CardTypeEnumScriptableObject>();

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

    public void CreateCard()
    {
        int cardValue = Random.Range(Card.CARD_VALUE_RANGE_EXCLUSIVE[0], Card.CARD_VALUE_RANGE_EXCLUSIVE[1]);
        int cardTypeIndex = Random.Range(0, availableCardTypes.Count);
        CardTypeEnumScriptableObject cardType = availableCardTypes[cardTypeIndex];

        while (hand.ContainSameValueInList(cardValue, cardType))
        {
            cardValue = Random.Range(Card.CARD_VALUE_RANGE_EXCLUSIVE[0], Card.CARD_VALUE_RANGE_EXCLUSIVE[1]);
            cardTypeIndex = Random.Range(0, availableCardTypes.Count);
            cardType = availableCardTypes[cardTypeIndex];
        }

        Card card = Instantiate(emptyCard);
        card.Instantiation(cardValue, cardType);


    }


}
