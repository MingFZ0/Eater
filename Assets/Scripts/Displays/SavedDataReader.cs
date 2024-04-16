using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class SavedDataReader : MonoBehaviour
{
    private string fileURL;
    [SerializeField] private string gameMode;
    [SerializeField] private TextMeshProUGUI eaterDisplay;
    [SerializeField] private TextMeshProUGUI roundDisplay;
    [SerializeField] private TextMeshProUGUI gameStateDisplay;
    [SerializeField] private TextMeshProUGUI scoreDisplay;

    private void Awake()
    {
        fileURL = Application.dataPath + "/SaveData.save";
    }

    private void Start()
    {
        string fileContent = File.ReadAllText(Application.dataPath + "/SaveData.save");
        SavedData savedData = JsonUtility.FromJson<SavedData>(fileContent);
        if (savedData == null) { return;}
        if (savedData.ContainsMode(gameMode) == false) { return; }
        Debug.Log("data found");
        ModeData modeData = savedData.GetModeData(gameMode);
        roundDisplay.text = "Rounds Lived: " + modeData.survivedRound;
        eaterDisplay.text = "Eaters Left: " + modeData.remainingEater;
        scoreDisplay.text = "Score: " + modeData.score;
        gameStateDisplay.text = "Game " + modeData.gameState;
    }
}
