using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

        private const float AnimatorDampTime = 0.1f;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.InputReader.ToggleTargetEvent += OnTarget;

            StateMachine.Animator.Play(_freeLookBlendTreeHash);
        }

        public override void Tick(float deltaTime)
        {
            var movementValue = StateMachine.InputReader.MovementValue;
            var movement = CalculateMovement();
            
            Move(movement * (StateMachine.FreeLookMovementSpeed * movementValue.magnitude), deltaTime);

            if (StateMachine.InputReader.MovementValue == Vector2.zero)
            {
                var currentValue = StateMachine.Animator.GetFloat(_freeLookSpeedHash);
                const float threshold = 0.001f;

                if (Mathf.Abs(currentValue) < threshold)
                    StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0);
                else
                    StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0, AnimatorDampTime, deltaTime);

                return;
            }


            StateMachine.Animator.SetFloat(_freeLookSpeedHash, movementValue.magnitude, AnimatorDampTime,
                deltaTime);
            FaceMovementDirection(movement, deltaTime);
        }


        public override void Exit()
        {
            StateMachine.InputReader.ToggleTargetEvent -= OnTarget;
        }

        private void OnTarget()
        {
            var targeter = StateMachine.Targeter;
            if (!targeter.SelectTarget()) return;

            switch (targeter.isLockedOn)
            {
                case false:
                    targeter.isLockedOn = true;
                    StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
                    break;
                case true:
                    targeter.isLockedOn = false;
                    break;
            }

            // if (!StateMachine.Targeter.SelectTarget()) return;
            //
            // switch (_isLockedOn)
            // {
            //     case false:
            //         StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            //         _isLockedOn = true;
            //         break;
            //     case true:
            //         _isLockedOn = false;
            //         break;
            // }
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