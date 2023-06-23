using Unity.VisualScripting;
using UnityEngine;

namespace Enemy.StateMachine.States
{
    public class EnemyAttack: EnemyBaseState
    {
        private float _speed, _imputMagnitude;
        private Vector3 _loocVector, _movementVector;
        
        private EnemyControllerSM _enemyControllerSm;
        private Vector3 _targetPoint, _directional;

        public EnemyAttack(EnemyControllerSM _stateMachine) : base("EnemyAttack", _stateMachine)
        {
            _enemyControllerSm = (EnemyControllerSM)stateMachine;
        }

        public override void Enter()
        {
            _enemyControllerSm.animator.SetBool("Move", true);
        }

        public override void UpdateLogic()
        {
           
        }

        public override void UpdatePhysics()
        {
            
        }
    }
}