using Unity.VisualScripting;

namespace StateMachines.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        private bool _isLockedOn = true;
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
            switch (_isLockedOn)
            {
                case true:
                    _isLockedOn = false;
                    StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                    break;
                case false:
                    _isLockedOn = true;
                    break;
            }
        }
    }
}
