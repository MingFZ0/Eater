using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Whenever you want to react to an event, attach this script to the corresponding gameobject
 */

public class GameEventListener : MonoBehaviour
{
    //GameEvent to listen to
    public GameEvent GameEvent;

    //Using the UnityEvent system to create a response that links to a method to activate when GameEvent gets raised
    public UnityEvent Response;

    private void OnEnable() {GameEvent.RegisterListener(this);}
    private void OnDisable() {GameEvent.UnregisterListener(this);}

    public void OnEventRaised() { Response.Invoke(); }
}
