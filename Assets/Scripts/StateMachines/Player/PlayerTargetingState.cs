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
            if (StateMachine.Targeter.CurrentTarget != null) return;
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
        }

        public override void Exit()
        {
            StateMachine.InputReader.ToggleTargetEvent -= OnCancel;
        }

        private void OnCancel()
        {
            var targeter = StateMachine.Targeter;
            targeter.Cancel();

            switch (targeter.isLockedOn)
            {
                case true:
                    targeter.isLockedOn = false;
                    StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                    break;
                case false:
                    targeter.isLockedOn = true;
                    break;
            }
        }
    }
}