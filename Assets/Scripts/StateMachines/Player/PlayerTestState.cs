using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace StateMachines.Player
{
    public class PlayerTestState : PlayerBaseState
    {
        public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            Vector2 movementValue = StateMachine.InputReader.MovementValue;
            Vector3 movement = CalculateMovement();

            StateMachine.Controller.Move(movement * CalculateMovementSpeed(CalculateMovementValue(movementValue)) *
                                         deltaTime);

            if (StateMachine.InputReader.MovementValue == Vector2.zero)
            {
                StateMachine.Animator.SetFloat("FreeLookSpeed", 0, 0.1f, deltaTime);
                return;
            }

            // Debug.Log(CalculateMovementValue(movementValue));
            StateMachine.Animator.SetFloat("FreeLookSpeed", CalculateMovementValue(movementValue), 0.1f, deltaTime);
            StateMachine.transform.rotation = Quaternion.LookRotation(movement);
        }

        public override void Exit()
        {
        }

        private Vector3 CalculateMovement()
        {
            var forward = StateMachine.MainCameraTransform.forward;
            var right = StateMachine.MainCameraTransform.right;

            forward.y = 0;
            right.y = 0;
            
            forward.Normalize();
            right.Normalize();

            return forward * StateMachine.InputReader.MovementValue.y +
                   right * StateMachine.InputReader.MovementValue.x;
        }

        // Custom method used to calculate the value for the Animator Blend Tree based on the values received from the
        // input.
        float CalculateMovementValue(Vector2 movementValue)
        {
            return Mathf.Abs(Mathf.Abs(movementValue.x) > Mathf.Abs(movementValue.y) ? movementValue.x : movementValue.y);
        }

        // Custom method that will alter the speed which the player moves based on the values received from the input.
        float CalculateMovementSpeed(float divisor)
        {
            return StateMachine.FreeLookMovementSpeed * divisor;
        }
    }
}