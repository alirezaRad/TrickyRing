using UnityEngine;
using System;
using Enums;

namespace ScriptableObjects.GameEvents
{
[CreateAssetMenu(menuName = "GameEvents/IntEvent")]
    public class IntEvent : ScriptableObject
    {
        public Action<int> OnEventRaised;
        public void Raise(int value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}