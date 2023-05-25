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
            Vector3 movement = new Vector3();

            movement.x = movementValue.x;
            movement.y = 0;
            movement.z = movementValue.y;

            StateMachine.Controller.Move(movement * StateMachine.FreeLookMovementSpeed * deltaTime);

            if (StateMachine.InputReader.MovementValue == Vector2.zero) return;
            
            StateMachine.transform.rotation = Quaternion.LookRotation(movement);
        }

        public override void Exit()
        {
        }
    }
}