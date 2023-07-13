using Unity.VisualScripting;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        private readonly int _targetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");

        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.InputReader.ToggleTargetEvent += OnCancel;

            StateMachine.Animator.Play(_targetingBlendTreeHash);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.Targeter.CurrentTarget == null)
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                StateMachine.Targeter.isLockedOn = false;
                return;
            }

            var movement = CalculateMovement();
            Move(movement * (StateMachine.TargetingMovementSpeed * StateMachine.InputReader.MovementValue.magnitude),
                deltaTime);

            FaceTarget();
        }

        public override void Exit()
        {
            StateMachine.InputReader.ToggleTargetEvent -= OnCancel;
        }

        private void OnCancel()
        {
            var targeter = StateMachine.Targeter;
            targeter.Cancel();

            if (targeter.isLockedOn == true)
            {
                targeter.isLockedOn = false;
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            }
        }

        private Vector3 CalculateMovement()
        {
            var movement = new Vector3();
            var transform = StateMachine.transform;

            movement += transform.right * StateMachine.InputReader.MovementValue.x;
            movement += transform.forward * StateMachine.InputReader.MovementValue.y;

            return movement;
        }
    }
}