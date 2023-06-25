using Unity.VisualScripting;
using UnityEngine;

namespace Enemy.StateMachine.States
{
    public class EnemyAttack: EnemyBaseState
    {
        private float _speedWalk, _speedRun, _realSpeed, _checkSpeed;
        private Transform _targetTransform, _thisTransform;
        private CharacterController _characterController;
        private EnemyControllerSM _enemyControllerSm;
       

        public EnemyAttack(EnemyControllerSM _stateMachine) : base("EnemyAttack", _stateMachine)
        {
            _enemyControllerSm = (EnemyControllerSM)stateMachine;
        }

        public override void Enter()
        {
            _enemyControllerSm.meshRenderer.enabled = true;
            _speedWalk = _enemyControllerSm.speedAnimation * _enemyControllerSm.walkSpeed;
            _speedRun = _enemyControllerSm.speedAnimation * _enemyControllerSm.runSpeed;
            _targetTransform = _enemyControllerSm.target;
            _thisTransform = _enemyControllerSm.thisTransform;
            _checkSpeed = _speedRun - _speedWalk;
            _characterController = _enemyControllerSm.characterController;
            _enemyControllerSm.animator.SetBool("Move", true);
        }

        public override void UpdateLogic()
        {
            
        }

        public override void UpdatePhysics()
        {
            _thisTransform.LookAt(new Vector3(_targetTransform.position.x, _thisTransform.position.y,_targetTransform.position.z));
            float direction = Vector3.Distance(_targetTransform.position, _thisTransform.position);
            if (Physics.Raycast(_thisTransform.position + Vector3.up, _thisTransform.forward ,
                    direction, _enemyControllerSm.stage))
            {
                _enemyControllerSm.playerLastCoordinate = _targetTransform.position;
                _enemyControllerSm.ChangeState(_enemyControllerSm.enemyFindObject);
                return;
            }
            _realSpeed = _speedWalk + _checkSpeed *  _enemyControllerSm.speedMove;
            Debug.Log(_realSpeed + " / " + _checkSpeed * _enemyControllerSm.speedMove);
            _characterController.Move(_thisTransform.forward * _realSpeed*Time.deltaTime);
        }
    }
}