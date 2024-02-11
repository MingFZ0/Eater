using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://unity.com/how-to/scriptableobject-based-runtime-set?clickref=1101lygDANiZ&utm_source=partnerize&utm_medium=affiliate&utm_campaign=unity_affiliate

public abstract class RuntimeSetSO<T> : ScriptableObject
{
    public List<T> Items = new List<T>();

    public void Add(T thing)
    {
        if (!Items.Contains(thing))
            Items.Add(thing);
    }

    public void Remove(T thing)
    {
        if (Items.Contains(thing))
            Items.Remove(thing);
    }
}