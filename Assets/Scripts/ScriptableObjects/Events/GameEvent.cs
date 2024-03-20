using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Scriptable object that is used to represent a specific GameEvent.
 * Similar to a runtimeset, this object keep track a list of all the gameeventlisteners
 * that are listening to the specific gameevent.
 * 
 * When the GameEvent gets raised by doing GameEvent.Raise(), the event will notify all
 * of its listeners BACKWARDS.
 */

[CreateAssetMenu(fileName = "GameEvents", menuName = "ScriptableObjects/GameEvents")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener) { listeners.Add(listener); }
    public void UnregisterListener(GameEventListener listener) { listeners.Remove(listener); }
}
