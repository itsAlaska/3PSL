namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        void Start()
        {
            SwitchState(new PlayerTestState(this));
        }
    }
}

