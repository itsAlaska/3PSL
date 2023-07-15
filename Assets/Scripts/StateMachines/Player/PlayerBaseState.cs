

using UnityEngine;

namespace StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {
        // State machine meant to be defined by the entity which inherits it
        protected PlayerStateMachine StateMachine;

        // Constructor method, requires that the player's state machine who's base state is being set be passed in the
        // method
        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }
        
        protected void Move(Vector3 motion, float deltaTime)
        {
            StateMachine.Controller.Move((StateMachine.ForceReceiver.Movement + motion) * deltaTime);
        }

        protected void FaceTarget()
        {
            if (StateMachine.Targeter.CurrentTarget == null) return;

            var lookPos =
                StateMachine.Targeter.CurrentTarget.transform.position - StateMachine.transform.position;
            lookPos.y = 0;

            StateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}

