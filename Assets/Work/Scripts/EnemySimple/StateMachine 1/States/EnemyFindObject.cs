using UnityEngine;

namespace Enemy.StateMachine.States
{
    public class EnemyFindObject: EnemyBaseState
    {
        private EnemyControllerSM _enemyControllerSm;
        private Transform _thisTransform;
        private Vector3 _targetPosition;
        private CharacterController _characterController;
        private float _speedWalk, _speedRun, _realSpeed, _checkSpeed;
        public EnemyFindObject(EnemyControllerSM stateMachine) : base("EnemyControllerSM", stateMachine)
        {
            _enemyControllerSm = (EnemyControllerSM)stateMachine;
        }

        public override void Enter()
        {
            _enemyControllerSm.animator.SetBool("Fight", false);
            _speedWalk = _enemyControllerSm.speedAnimation * _enemyControllerSm.walkSpeed;
            _speedRun = _enemyControllerSm.speedAnimation * _enemyControllerSm.runSpeed;
            _checkSpeed = _speedRun - _speedWalk;
            _characterController = _enemyControllerSm.characterController;
            _enemyControllerSm.animator.SetBool("Move", true);
            _thisTransform = _enemyControllerSm.thisTransform;
            _targetPosition = _enemyControllerSm.playerLastCoordinate;
        }
        
        public override void UpdatePhysics()
        {
            _thisTransform.LookAt(new Vector3(_targetPosition.x, _thisTransform.position.y,_targetPosition.z));
            
            for (int i = 0; i < EnemyController.instance.playerList.Count; i++)
            {
                if (Vector3.Distance(EnemyController.instance.playerList[i].position,
                        _enemyControllerSm.thisTransform.position) < _enemyControllerSm.radiusLoock)
                {
                    var direction = EnemyController.instance.playerList[i].position -
                                    _enemyControllerSm.thisTransform.position;
                    if (!Physics.Raycast(_enemyControllerSm.thisTransform.position + Vector3.up, direction.normalized,
                            _enemyControllerSm.radiusLoock, _enemyControllerSm.stage))
                    {
                        _enemyControllerSm.target = EnemyController.instance.playerList[i];
                        _enemyControllerSm.ChangeState(_enemyControllerSm.attack);
                        return;
                    }
                }
            }
            float directionToPoint = Vector3.Distance(_targetPosition, _thisTransform.position);
            if (directionToPoint < 1)
            {
                _enemyControllerSm.ChangeState(_enemyControllerSm.idle);
                return;
            }
            _realSpeed = _speedWalk + _checkSpeed *  _enemyControllerSm.speedMove;
            Debug.Log(_realSpeed + " / " + _checkSpeed * _enemyControllerSm.speedMove);
            _characterController.Move(_thisTransform.forward * _realSpeed*Time.deltaTime);
        }
    }
}