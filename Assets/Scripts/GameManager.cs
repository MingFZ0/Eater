using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] HandManager _handManager;
    [SerializeField] EaterManager _eaterManger;
    [SerializeField] TurnManager _turnManager;
    [SerializeField] PrizeCardManager _prizeCardManager;

    public enum PhaseType
    {
        DrawPhrase,
        FeedPhrase,
        EndPhrase
    }
    private PhaseType _gamePhase;
    public PhaseType GamePhase
    {
        get {return _gamePhase;}
        set {throw new System.Exception("You tried modifying the View-Only global variable GamePhase. If you want to modify it, modify the private _gamePhase within GameManager's methods.");}
    }

    public int GameRound { get { return _gameRound; } }
    private int _gameRound;
    public int Score { get { return _score; } }
    private int _score;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void phaseInit(PhaseType gamePhase)
    {
        if (gamePhase == PhaseType.DrawPhrase) {
        
        }
        else if (gamePhase == PhaseType.FeedPhrase) { 
        
        }
        else if (gamePhase == PhaseType.EndPhrase) { 
        
        }
        else { throw new System.Exception("Unable to recognize game phase"); }
    }
}
