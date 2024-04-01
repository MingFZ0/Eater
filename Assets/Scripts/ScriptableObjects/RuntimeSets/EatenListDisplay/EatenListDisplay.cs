using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "EatenListDisplay", menuName = "ScriptableObjects/RuntimeSet/EatenListDisplay")]
public class EatenListDisplay : ScriptableObject
{
    [SerializeField] private float displayBoxWidth;
    [SerializeField] private float displayBoxYCoord;

    [SerializeField] private CardsInUse cardsInUse;
    [SerializeField] public List<GameObject> currentList;
    [SerializeField] private GameObject displayCard;

    [SerializeField] bool displayBoxOutline;

    /// <summary>
    /// Updates the currentList and displays it.
    /// </summary>
    /// <param name="eatingList"></param>
    public void updateDisplay(List<string> eatingList)
    {
        clearCurrentList();

        float currentZCord = -4 + eatingList.Count;

        float distanceBetweenEachCard = displayBoxWidth / (eatingList.Count);
        float currentIteratingPos = distanceBetweenEachCard - (displayBoxWidth / 2);
        currentIteratingPos -= (distanceBetweenEachCard / 2);

        foreach (string cardName in eatingList)
        {
            Sprite cardSprite = cardsInUse.getCardSprite(cardName);
            GameObject card = Instantiate<GameObject>(displayCard);

            card.transform.position = new Vector3(currentIteratingPos, displayBoxYCoord, currentZCord);
            card.GetComponent<SpriteRenderer>().sprite = cardSprite;
            currentIteratingPos += distanceBetweenEachCard;
            currentZCord--;
            currentList.Add(card);
        }
    }

    public void updateDisplay(List<string> feedingList, List<string> eatenList)
    {
        clearCurrentList();

        float currentZCord = -4 + eatenList.Count;

        float distanceBetweenEachCard = displayBoxWidth / (eatenList.Count + feedingList.Count);
        float currentIteratingPos = distanceBetweenEachCard - (displayBoxWidth / 2);
        currentIteratingPos -= (distanceBetweenEachCard / 2);

        foreach (string cardName in eatenList)
        {
            Sprite cardSprite = cardsInUse.getCardSprite(cardName);
            GameObject card = Instantiate<GameObject>(displayCard);

            card.transform.position = new Vector3(currentIteratingPos, (cardSprite.textureRect.height /100) + displayBoxYCoord, currentZCord);
            card.GetComponent<SpriteRenderer>().sprite = cardSprite;
            card.GetComponentInChildren<SpriteRenderer>().color = Color.gray;
            currentIteratingPos += distanceBetweenEachCard;
            currentZCord--;
            currentList.Add(card);
        }

        foreach (string cardName in feedingList)
        {
            Sprite cardSprite = cardsInUse.getCardSprite(cardName);
            GameObject card = Instantiate<GameObject>(displayCard);

            card.transform.position = new Vector3(currentIteratingPos,(cardSprite.textureRect.height / 100) + displayBoxYCoord, currentZCord);
            card.GetComponent<SpriteRenderer>().sprite = cardSprite;
            currentIteratingPos += distanceBetweenEachCard;
            currentZCord--;
            currentList.Add(card);
        }
    }


    public void clearCurrentList()
    {
        if (currentList != null)
        {
            foreach (GameObject obj in currentList)
            {
                Destroy(obj);
            }

            currentList.Clear();
        }
    }

    private void OnValidate()
    {
        clearCurrentList();
    }

    // === Unity Editor EatenList Display === //
    private void OnEnable()
    {
        #if UNITY_EDITOR
        SceneView.duringSceneGui += OnSceneUpdate;
        #endif
    }
    private void OnDisable()
    {
        #if UNITY_EDITOR
        SceneView.duringSceneGui -= OnSceneUpdate;
        #endif
    }

    #if UNITY_EDITOR
    private void OnSceneUpdate(SceneView sceneView)
    {
        if (displayBoxOutline == false) { return; }
        Handles.color = Color.green;
        Sprite cardSprite = displayCard.GetComponent<SpriteRenderer>().sprite;
        Handles.DrawSolidRectangleWithOutline(new Rect(0 - (displayBoxWidth / 2), displayBoxYCoord + (cardSprite.textureRect.height / 100), displayBoxWidth, 3), Color.clear, Color.green);
    }
    #endif
}
