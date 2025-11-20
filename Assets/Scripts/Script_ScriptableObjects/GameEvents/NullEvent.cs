using UnityEngine;
using System;
using Enums;

namespace ScriptableObjects.GameEvents
{
[CreateAssetMenu(menuName = "GameEvents/NullEvent")]
    public class NullEvent : ScriptableObject
    {
        public Action OnEventRaised;
        public void Raise()
        {
            OnEventRaised?.Invoke();
        }
    }
}
