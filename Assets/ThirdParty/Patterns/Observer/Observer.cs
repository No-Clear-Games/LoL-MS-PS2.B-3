using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NoClearGames.Patterns.Observer
{
    public class Observer<T> where T : Object
    {
       public List<UnityAction<T>> Observers = new List<UnityAction<T>>();

        public void Add(UnityAction<T> newItem)
        {
            if(!Observers.Contains(newItem))  
                Observers.Add(newItem);
        }

        public void Remove(UnityAction<T> item)
        {
            if (!Observers.Contains(item))
                Observers.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (Observers.Count >= index)
                Observers.RemoveAt(index);
        }

        public void Notify(T value)
        {
            foreach (UnityAction<T> item in Observers)
                item(value);
        }
    }
}
