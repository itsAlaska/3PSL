using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

        private const float AnimatorDampTime = 0.1f;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            var movementValue = StateMachine.InputReader.MovementValue;
            var movement = CalculateMovement();

            StateMachine.Controller.Move(movement * (StateMachine.FreeLookMovementSpeed * movementValue.magnitude) *
                                         deltaTime);

            if (StateMachine.InputReader.MovementValue == Vector2.zero)
            {
                var currentValue = StateMachine.Animator.GetFloat(_freeLookSpeedHash);
                const float threshold = 0.001f;

                if (Mathf.Abs(currentValue) < threshold)
                {
                    StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0);
                }
                else
                {
                    StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
                }
                
                Debug.Log($"In Vector2.zero:\n{StateMachine.Animator.GetFloat(_freeLookSpeedHash)}");
                return;
            }
            

            StateMachine.Animator.SetFloat(_freeLookSpeedHash, movementValue.magnitude, AnimatorDampTime,
                deltaTime);
            FaceMovementDirection(movement, deltaTime);
            Debug.Log(StateMachine.Animator.GetFloat(_freeLookSpeedHash));
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
    }
}