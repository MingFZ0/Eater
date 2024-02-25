using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseDisplay : MonoBehaviour
{
    [SerializeField] GameVariables gameVars;
    [SerializeField] TextMeshProUGUI roundDisplay;
    [SerializeField] TextMeshProUGUI turnDisplay;
    [SerializeField] TextMeshProUGUI phaseDisplay;
    [SerializeField] TextMeshProUGUI scoreDisplay;

    public void updateDisplay()
    {
        roundDisplay.text = "Round " + gameVars.Round;
        turnDisplay.text = "Turn " + gameVars.Turn;

        phaseDisplay.text = gameVars.GetGamePhase().GetDisplayName();
        scoreDisplay.text = "Score: " + gameVars.GetScore();

    }
}
