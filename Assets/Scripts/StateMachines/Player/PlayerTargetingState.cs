using Unity.VisualScripting;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.InputReader.ToggleTargetEvent += OnCancel;
        }

        public override void Tick(float deltaTime)
        {
            
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