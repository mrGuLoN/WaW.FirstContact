using Mirror;

namespace Player.StateMachine
{
    public class PlayerStateMachine : NetworkBehaviour
    {
        private PlayerBaseState _currentState;

        
        private void Start()
        {
            _currentState = GetInitialState();
            if (_currentState != null) _currentState.Enter();
        }
      
        private void Update()
        {
            if (_currentState != null)
            {
                if (!isLocalPlayer) return;
                _currentState.UpdateLogic();
            }
        } 
        
        private void LateUpdate()
        {
            if (_currentState != null)
            {
                if (!isLocalPlayer) return;
                _currentState.UpdatePhysics();
            }
        }

        public void ChangeState(PlayerBaseState newState)
        {
            if (_currentState!=null)_currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        protected virtual PlayerBaseState GetInitialState()
        {
            return null;
        }

    }
}