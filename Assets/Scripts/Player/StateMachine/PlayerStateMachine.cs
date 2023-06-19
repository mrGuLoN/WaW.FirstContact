using System;
using Mirror;
using Player.StateMachine.States;
using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerStateMachine : NetworkBehaviour
    {
        private PlayerBaseState _currentState;

        private void Start()
        {
            if (!isLocalPlayer) return;
            _currentState = GetInitialState();
            if (_currentState != null) _currentState.Enter();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (_currentState != null)
            {
                _currentState.UpdateLogic();
            }
        }

        private void LateUpdate()
        {
            if (!isLocalPlayer) return;
            if (_currentState != null)
            {
                _currentState.UpdatePhysics();
            }
        }

        public void ChangeState(PlayerBaseState newState)
        {
            if (!isLocalPlayer) return;
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