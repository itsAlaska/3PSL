using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace StateMachines.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        private readonly int _targetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
        private readonly int _targetingForwardHash = Animator.StringToHash("TargetingForwardSpeed");
        private readonly int _targetingRightHash = Animator.StringToHash("TargetingRightSpeed");

        private const float AnimatorDampTime = 0.1f;

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
            if (StateMachine.InputReader.IsAttacking)
            {
                StateMachine.SwitchState(new PlayerAttackingState(StateMachine, 0));
                return;
            }
            
            if (StateMachine.Targeter.CurrentTarget == null)
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                StateMachine.Targeter.isLockedOn = false;
                return;
            }

            var movement = CalculateMovement();
            Move(movement * (StateMachine.TargetingMovementSpeed * StateMachine.InputReader.MovementValue.magnitude),
                deltaTime);

            UpdateAnimator(deltaTime);

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

        private void UpdateAnimator(float deltaTime)
        {
            var animator = StateMachine.Animator;
            var movementValue = StateMachine.InputReader.MovementValue;
            var currentForwardValue = animator.GetFloat(_targetingForwardHash);
            var currentRightValue = animator.GetFloat(_targetingForwardHash);
            const float threshold = 0.001f;
            Debug.Log($"movementValue.y: {movementValue.y}");
            Debug.Log($"movementValue.x: {movementValue.x}");

            if (movementValue.y == 0)
            {
                if (Mathf.Abs(currentForwardValue) < threshold) animator.SetFloat(_targetingForwardHash, 0);
                else animator.SetFloat(_targetingForwardHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                animator.SetFloat(_targetingForwardHash, movementValue.y, AnimatorDampTime, deltaTime);
            }

            if (movementValue.x == 0)
            {
                if (Mathf.Abs(currentRightValue) < threshold) animator.SetFloat(_targetingRightHash, 0);
                else animator.SetFloat(_targetingRightHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                animator.SetFloat(_targetingRightHash, movementValue.x, AnimatorDampTime, deltaTime);
            }

            // if (movementValue == Vector2.zero)
            // {
            //     var currentForwardValue = animator.GetFloat(_targetingForwardHash);
            //     var currentRightValue = animator.GetFloat(_targetingRightHash);
            //     const float threshold = 0.001f;
            //
            //     if (Mathf.Abs(currentForwardValue) < threshold && Mathf.Abs(currentRightValue) < threshold)
            //     {
            //         animator.SetFloat(_targetingForwardHash, 0);
            //         animator.SetFloat(_targetingRightHash, 0);
            //     }
            //     else
            //     {
            //         animator.SetFloat(_targetingForwardHash, 0, AnimatorDampTime, deltaTime);
            //         animator.SetFloat(_targetingRightHash, 0, AnimatorDampTime, deltaTime);
            //     }
            // }
            // else
            // {
            //     animator.SetFloat(_targetingForwardHash, movementValue.y, AnimatorDampTime, deltaTime);
            //     animator.SetFloat(_targetingRightHash, movementValue.x, AnimatorDampTime, deltaTime);
            // }
        }
    }
}