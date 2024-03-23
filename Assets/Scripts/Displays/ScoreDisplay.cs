using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] GameVariables gameVars;
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] TextMeshProUGUI roundDisplay;

    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay.text = "Score: " + gameVars.GetScore();
        roundDisplay.text = "Round Survived: " + gameVars.Round;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
