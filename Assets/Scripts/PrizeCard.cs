using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeCard : MonoBehaviour
{
    // Start is called before the first frame update

    Sprite cardSprite;

    public int CardValue;
    public CardProperties.cardTypes CardType;

    System.Random randomNumGen = new System.Random();

    public bool IsFaceUp = false;
    [SerializeField] Color faceUpColor;
    [SerializeField] Color faceDownColor;

    SpriteRenderer spriteRenderer;

    [SerializeField] TextMesh textDisplay;

    private void Awake()
    {
        int cardTypeID = randomNumGen.Next(0, 4);
        CardType = (CardProperties.cardTypes)cardTypeID;
        CardValue = randomNumGen.Next(1, 14);
        this.name = $"{CardValue.ToString()}_{CardType}";
    }

    private void OnDestroy()
    {
        PrizeCardManager.PrizeCardsList.Remove(this);
    }
    //void OnEnable() => HandManager.CardsInHand.Add(this);
    //void OnDisable() => HandManager.CardsInHand.Remove(this);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textDisplay.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsFaceUp)
        { 
            spriteRenderer.color = faceDownColor;
            textDisplay.text = "";
        }
        else 
        { 
            spriteRenderer.color = faceUpColor;
            textDisplay.text = CardValue.ToString();
        }
    }

    private void OnMouseDown()
    {
        //Destroy(this);
        
    }
}
