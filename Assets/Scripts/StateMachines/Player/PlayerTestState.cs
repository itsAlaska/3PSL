using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTestState : PlayerBaseState
    {
        public float Time = 5;

        public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enter");
        }

        public override void Tick(float deltaTime)
        {
            Time -= deltaTime;
            if (Time <= 0)
            {
                StateMachine.SwitchState(new PlayerTestState(StateMachine));
            }
            Debug.Log(Time);
        }

        public override void Exit()
        {
            Debug.Log("Exit");
        }
    }
}