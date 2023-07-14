namespace StateMachines.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private readonly Attack _attack;

        public PlayerAttackingState(PlayerStateMachine stateMachine, int attackId) : base(stateMachine)
        {
            _attack = stateMachine.Attacks[attackId];
        }

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, .1f);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}