using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTestState : PlayerBaseState
    { 
        float _time;

        public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enter");
            StateMachine.InputReader.JumpEvent += OnJump;
        }

        public override void Tick(float deltaTime)
        {
            _time += deltaTime;
            Debug.Log(_time);
        }

        public override void Exit()
        {
            Debug.Log("Exit");
            StateMachine.InputReader.JumpEvent -= OnJump;
        }

        void OnJump()
        {
            StateMachine.SwitchState(new PlayerTestState(StateMachine));
        }
    }
}