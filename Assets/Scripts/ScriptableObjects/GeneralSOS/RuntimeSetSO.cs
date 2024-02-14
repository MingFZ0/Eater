using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://unity.com/how-to/scriptableobject-based-runtime-set?clickref=1101lygDANiZ&utm_source=partnerize&utm_medium=affiliate&utm_campaign=unity_affiliate

public abstract class RuntimeSetSO<T> : ScriptableObject
{
    [SerializeField] protected List<T> items = new List<T>();

    public abstract void Add(T thing);
    //{
    //    if (!Items.Contains(thing))
    //        Items.Add(thing);
    //}

    public abstract void Remove(T thing);
    //{
    //    if (Items.Contains(thing))
    //        Items.Remove(thing);
    //}

    public abstract T GetValue(int index);
    //{
    //    return Items[index];
    //}

}