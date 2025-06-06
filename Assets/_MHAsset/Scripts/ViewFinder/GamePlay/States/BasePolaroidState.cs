namespace MH.GamePlay.States
{
    public class BasePolaroidState : State<EPolaroidState>
    {
        protected PolaroidManager polaroidManager;
        
        public BasePolaroidState(StateMachine<EPolaroidState> stateMachine, PolaroidManager manager) : base(stateMachine)
        {
            polaroidManager = manager;
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}