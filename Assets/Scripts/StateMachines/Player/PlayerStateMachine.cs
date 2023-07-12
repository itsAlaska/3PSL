using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public InputReader InputReader { get; private set; }
        
        void Start()
        {
            if (Camera.main != null) MainCameraTransform = Camera.main.transform;

            SwitchState(new PlayerFreeLookState(this));
        }
    }
}

