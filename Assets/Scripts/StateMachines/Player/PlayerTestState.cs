using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTestState : PlayerBaseState
    {
        public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            Vector2 movementValue = StateMachine.InputReader.MovementValue;
            Vector3 movement = new Vector3
            {
                x = movementValue.x,
                y = 0,
                z = movementValue.y
            };
            StateMachine.transform.Translate(movement * deltaTime);
        }

        public override void Exit()
        {
        }
    }
}