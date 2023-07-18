using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        // private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        // private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private readonly int _freeLookForwardHash = Animator.StringToHash("FreeLookForwardSpeed");
        private readonly int _freeLookRightHash = Animator.StringToHash("FreeLookRightSpeed");
        // private readonly int _freeLookBlendTreeHash2 = Animator.StringToHash("FreeLookBlendTree2");
        private readonly int _freeLookBlendTreeHash3 = Animator.StringToHash("FreeLookBlendTree3");
        
        private const float AnimatorDampTime = 0.1f;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.InputReader.ToggleTargetEvent += OnTarget;

            StateMachine.Animator.Play(_freeLookBlendTreeHash3);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.InputReader.IsAttacking)
            {
                FaceTarget();
                StateMachine.SwitchState(new PlayerAttackingState(StateMachine, 0));
                return;
            }

            var movementValue = StateMachine.InputReader.MovementValue;
            var movement = CalculateMovement();
            var movementSpeed = StateMachine.FreeLookMovementSpeed;
            // var movementSpeed = StateMachine.FreeLookMovementSpeed * CalculateSpeedReduction(movementValue);

            // Move(movement * (StateMachine.FreeLookMovementSpeed * movementValue.magnitude), deltaTime);

            // if (StateMachine.InputReader.MovementValue == Vector2.zero)
            // {
            //     var currentValue = StateMachine.Animator.GetFloat(_freeLookSpeedHash);
            //     const float threshold = 0.001f;
            //
            //     if (Mathf.Abs(currentValue) < threshold)
            //         StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0);
            //     else
            //         StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            //
            //     FaceCameraDirection(StateMachine.MainCameraTransform.forward, deltaTime);
            //     return;
            // }
            //
            //
            // StateMachine.Animator.SetFloat(_freeLookSpeedHash, movementValue.magnitude, AnimatorDampTime,
            //     deltaTime);
            // // FaceMovementDirection(movement, deltaTime);
            Move(movement * (movementSpeed * movementValue.magnitude), deltaTime);
            UpdateAnimator(deltaTime);

            FaceCameraDirection(StateMachine.MainCameraTransform.forward, deltaTime);
        }


        public override void Exit()
        {
            StateMachine.InputReader.ToggleTargetEvent -= OnTarget;
        }

        private void OnTarget()
        {
            var targeter = StateMachine.Targeter;
            if (!targeter.SelectTarget()) return;

            if (targeter.isLockedOn == false)
            {
                targeter.isLockedOn = true;
                StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            }
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

        // private void FaceMovementDirection(Vector3 movement, float deltaTime)
        // {
        //     StateMachine.transform.rotation =
        //         Quaternion.Lerp(StateMachine.transform.rotation,
        //             Quaternion.LookRotation(movement),
        //             deltaTime * StateMachine.RotationDamping);
        // }

        private void FaceCameraDirection(Vector3 cameraTransform, float deltaTime)
        {
            var direction = cameraTransform;
            direction.y = 0;

            StateMachine.transform.rotation =
                Quaternion.Lerp(StateMachine.transform.rotation,
                    Quaternion.LookRotation(direction),
                    deltaTime * StateMachine.RotationDamping);
        }

        private void UpdateAnimator(float deltaTime)
        {
            var animator = StateMachine.Animator;
            var movementValue = StateMachine.InputReader.MovementValue;
            var currentForwardValue = animator.GetFloat(_freeLookForwardHash);
            var currentRightValue = animator.GetFloat(_freeLookRightHash);
            const float threshold = 0.001f;

            if (movementValue.y == 0)
            {
                if (Mathf.Abs(currentForwardValue) < threshold) animator.SetFloat(_freeLookForwardHash, 0);
                else animator.SetFloat(_freeLookForwardHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                animator.SetFloat(_freeLookForwardHash, movementValue.y, AnimatorDampTime, deltaTime);
            }

            if (movementValue.x == 0)
            {
                if (Mathf.Abs(currentRightValue) < threshold) animator.SetFloat(_freeLookRightHash, 0);
                else animator.SetFloat(_freeLookRightHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                animator.SetFloat(_freeLookRightHash, movementValue.x, AnimatorDampTime, deltaTime);
            }
        }
        
        private float CalculateSpeedReduction(Vector2 movementValue)
        {
            return movementValue.y switch
            {
                < -.7f => .5f,
                < -.5f => .6f,
                < -.3f => .8f,
                < 0 => .9f,
                _ => 1
            };
        }
    }
}