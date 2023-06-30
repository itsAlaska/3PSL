using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTestState : PlayerBaseState
    {
        private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

        private const float AnimatorDampTime = 0.1f;

        public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            var movementValue = StateMachine.InputReader.MovementValue;
            var movement = CalculateMovement();

            StateMachine.Controller.Move(movement * CalculateMovementSpeed(CalculateMovementValue(movementValue)) *
                                         deltaTime);

            if (StateMachine.InputReader.MovementValue == Vector2.zero)
            {
                StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
                return;
            }

            StateMachine.Animator.SetFloat(_freeLookSpeedHash, CalculateMovementValue(movementValue), AnimatorDampTime,
                deltaTime);
            FaceMovementDirection(movement, deltaTime);
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

        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            StateMachine.transform.rotation =
                Quaternion.Lerp(StateMachine.transform.rotation, 
                    Quaternion.LookRotation(movement),
                    deltaTime * StateMachine.RotationDamping);
        }

        // Custom method used to calculate the value for the Animator Blend Tree based on the values received from the
        // input.
        private float CalculateMovementValue(Vector2 movementValue)
        {
            return Mathf.Abs(
                Mathf.Abs(movementValue.x) > Mathf.Abs(movementValue.y) ? movementValue.x : movementValue.y);
        }

        // Custom method that will alter the speed which the player moves based on the values received from the input.
        private float CalculateMovementSpeed(float divisor)
        {
            return StateMachine.FreeLookMovementSpeed * divisor;
        }
    }
}