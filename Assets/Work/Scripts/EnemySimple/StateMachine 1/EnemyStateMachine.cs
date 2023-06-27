using Mirror;

namespace Enemy.StateMachine
{
    public class EnemyStateMachine : NetworkBehaviour
    {
        private EnemyBaseState _currentState;

        
        private void Start()
        {
            _currentState = GetInitialState();
            if (_currentState != null) _currentState.Enter();
        }

        public void UpdateLogic()
        {
            if (_currentState != null)
            {
                _currentState.UpdateLogic();
            }
        } 
        
        public void UpdatePhysics()
        {
            if (_currentState != null)
            {
                _currentState.UpdatePhysics();
            }
        }

        public void ChangeState(EnemyBaseState newState)
        {
            if (_currentState!=null)_currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        protected virtual EnemyBaseState GetInitialState()
        {
            return null;
        }

    }
}