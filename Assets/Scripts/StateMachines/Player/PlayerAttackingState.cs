using Unity.VisualScripting;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
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
            StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
        }

        public override void Tick(float deltaTime)
        {
            var movementValue = StateMachine.InputReader.MovementValue;
            var movement = CalculateMovement();
            var movementSpeed = StateMachine.FreeLookMovementSpeed;
            
            Move(movement * (movementSpeed * movementValue.magnitude), deltaTime);
            // Move(deltaTime);
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
            
            FaceCameraDirection(StateMachine.MainCameraTransform.forward, deltaTime);
        }

        public override void Exit()
        {
            StateMachine.Animator.SetLayerWeight(1, 0);
        }

        private float GetNormalizedTime()
        {
            var currentInfo = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var nextInfo = StateMachine.Animator.GetNextAnimatorStateInfo(0);

            if (StateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
                return nextInfo.normalizedTime;

            if (!StateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
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
    }
}