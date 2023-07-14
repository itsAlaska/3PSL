using System;
using UnityEngine;

namespace StateMachines.Player
{
    [Serializable]
    public class Attack
    {
        [field: SerializeField] public string AnimationName { get; private set; }
    }
}
