using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhaseEnum
{
    DrawPhase,      //when the player draws
    ActionPhase,    //Where the feeding, discarding, and everything else occurs
    EndPhase        //When the player flips over a prize card, the game switches to the endphase.
                    //The endphase is meant to be a calculating phase for the game to do calculations and checks, and after it is done, it moves back to the drawphase.
        

}
