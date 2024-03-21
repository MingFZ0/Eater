using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PrizeCardSpawner : MonoBehaviour
{
    [SerializeField] private float displayBoxHeight;

    [SerializeField] private PrizeCard card;
    [SerializeField] private CardsInHand hand;
    private List<CardTypeEnumScriptableObject> availableCardTypes;

    [SerializeField] private TreasureCard treasure;

    [SerializeField] private GameVariables gameVars;
    [SerializeField] private CardsInUse cardsInUse;

    [SerializeField] private bool showDisplayBox;

    public void Awake()
    {
        availableCardTypes = gameVars.GetAvailableCardTypes();
    }
    private void Start()
    {
        RoundStartSpawnPrize();
    }

    private void OnDrawGizmos()
    {
        if (!showDisplayBox) { return; }
        float cardWidth = card.GetComponent<BoxCollider2D>().size.x + 1;
        float cardHeight = card.GetComponent<BoxCollider2D>().size.y;
        Handles.DrawSolidRectangleWithOutline(new Rect((transform.position.x - (cardWidth/ 2 )), transform.position.y + cardHeight, cardWidth, (displayBoxHeight * -2f)), Color.clear, Color.green);
    }


    public void SpawnCard(Vector3 spawnPos)
    {
        PrizeCard prizeCard = cardsInUse.CreatePrizeCard(spawnPos);
    }

    public void RoundStartSpawnPrize()
    {
        int tmpConstant = 20;

        float yCoord = transform.position.y;
        float zCoord = transform.position.z + tmpConstant ;
        int amountNeeded = gameVars.GetINITIALPRIZE() + gameVars.Round;

        TreasureCard treasureCard = Instantiate(treasure, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1 + tmpConstant), new Quaternion());
        treasureCard.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1 + tmpConstant);

        float distanceBetweenEachCard = displayBoxHeight / (amountNeeded - 1);

        for (int i = 0; i < amountNeeded; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, yCoord, zCoord);
            SpawnCard(spawnPos);

            yCoord -= distanceBetweenEachCard;
            zCoord -= 1;
        }
    }
}
