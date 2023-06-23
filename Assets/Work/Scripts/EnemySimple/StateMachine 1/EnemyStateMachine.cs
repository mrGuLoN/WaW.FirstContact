using Mirror;

namespace Enemy.StateMachine
{
    public class EnemyStateMachine : NetworkBehaviour
    {
        private EnemyBaseState _currentState;

        
        private void Start()
        {
            if (!isLocalPlayer) return;
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