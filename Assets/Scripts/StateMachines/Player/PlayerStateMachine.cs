using Combat.Targeting;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Targeter Targeter { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationDamping { get; private set; }
        
        public Transform MainCameraTransform { get; private set; }

        void Start()
        {
            if (Camera.main != null) MainCameraTransform = Camera.main.transform;

            SwitchState(new PlayerFreeLookState(this));
        }
    }
}