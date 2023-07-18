using Unity.VisualScripting;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private readonly int _freeLookForwardHash = Animator.StringToHash("FreeLookForwardSpeed");
        private readonly int _freeLookRightHash = Animator.StringToHash("FreeLookRightSpeed");
        private const float AnimatorDampTime = 0.1f;
        
        private float _previousFrameTime;
        private bool _alreadyAppliedForce;

        private readonly Attack _attack;

        public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
        {
            _attack = stateMachine.Attacks[attackIndex];
        }

        public override void Enter()
        {
            StateMachine.Animator.SetLayerWeight(1, 1);
            StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration, 1);
        }

        public override void Tick(float deltaTime)
        {
            var movementValue = StateMachine.InputReader.MovementValue;
            var movement = CalculateMovement();
            var movementSpeed = StateMachine.FreeLookMovementSpeed;
            
            Move(movement * (movementSpeed * movementValue.magnitude), deltaTime);
            // Move(deltaTime);
            FaceCameraDirection(StateMachine.MainCameraTransform.forward, deltaTime);
            FaceTarget();

            var normalizedTime = GetNormalizedTime();

            if (normalizedTime < 1)
            {
                if (normalizedTime >= _attack.ForceTime) TryApplyForce();
                if (StateMachine.InputReader.IsAttacking) TryComboAttack(normalizedTime);
            }
            else
            {
                if (StateMachine.Targeter.CurrentTarget != null)
                {
                    StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
                }
                else
                {
                    StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                }
            }

            _previousFrameTime = normalizedTime;
            
            UpdateAnimator(deltaTime);
        }

        public override void Exit()
        {
            StateMachine.Animator.SetLayerWeight(1, 0);
        }

        private float GetNormalizedTime()
        {
            var currentInfo = StateMachine.Animator.GetCurrentAnimatorStateInfo(1);
            var nextInfo = StateMachine.Animator.GetNextAnimatorStateInfo(1);

            if (StateMachine.Animator.IsInTransition(1) && nextInfo.IsTag("Attack"))
                return nextInfo.normalizedTime;

            if (!StateMachine.Animator.IsInTransition(1) && currentInfo.IsTag("Attack"))
                return currentInfo.normalizedTime;

            return 0;
        }

        private void TryComboAttack(float normalizedTime)
        {
            if (_attack.ComboStateIndex == -1) return;
            if (normalizedTime < _attack.ComboAttackTime) return;
            StateMachine.SwitchState(
                new PlayerAttackingState
                (
                    StateMachine,
                    _attack.ComboStateIndex
                )
            );
        }

        private void TryApplyForce()
        {
            if (_alreadyAppliedForce) return;
            StateMachine.ForceReceiver.AddForce(StateMachine.transform.forward * _attack.Force);
            _alreadyAppliedForce = true;
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
    }
}