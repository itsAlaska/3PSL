using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private float _previousFrameTime;

        private readonly Attack _attack;

        public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
        {
            _attack = stateMachine.Attacks[attackIndex];
        }

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FaceTarget();

            var normalizedTime = GetNormalizedTime();

            if (normalizedTime > _previousFrameTime && normalizedTime < 1)
            {
                if (StateMachine.InputReader.IsAttacking) TryComboAttack(normalizedTime);
            }
            else
            {
                //todo
            }

            _previousFrameTime = normalizedTime;
        }

        public override void Exit()
        {
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
            Debug.Log(_attack.AnimationName);
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
    }
}