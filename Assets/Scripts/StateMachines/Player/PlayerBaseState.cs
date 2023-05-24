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
    }
}

