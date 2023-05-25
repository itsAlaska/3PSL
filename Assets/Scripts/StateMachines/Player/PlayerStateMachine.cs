using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
        
        void Start()
        {
            SwitchState(new PlayerTestState(this));
        }
    }
}

