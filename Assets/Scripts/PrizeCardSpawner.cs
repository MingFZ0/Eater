using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeCardSpawner : MonoBehaviour
{
    [SerializeField] private PrizeCard card;
    [SerializeField] private CardsInHand hand;
    private List<CardTypeEnumScriptableObject> availableCardTypes;

    [SerializeField] private GameVariables gameVars;

    public void Awake()
    {
        availableCardTypes = gameVars.GetAvailableCardTypes();
    }
    private void Start()
    {
        RoundStartSpawnPrize();
    }

    public void SpawnCard(Vector3 spawnPos)
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

        PrizeCard prizeCard = Instantiate(card, spawnPos, new Quaternion());
        prizeCard.Instantiation(cardValue, cardType);
    }

    public void RoundStartSpawnPrize()
    {
        float yCoord = transform.position.y;
        int amountNeeded = gameVars.GetINITIALPRIZE() + gameVars.Round;
        for (int i = 0; i < amountNeeded; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, yCoord, transform.position.z);
            SpawnCard(spawnPos);
            yCoord -= 1;

        }
    }
}
