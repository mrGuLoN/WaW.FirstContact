using DG.Tweening;
using UnityEngine;

namespace Enemy.StateMachine.States
{
    public class EnemyIdle : EnemyBaseState
    {
        private float _speed, _imputMagnitude;
        private Vector3 _movementVector;
        private EnemyControllerSM _enemyControllerSm;

        public EnemyIdle(EnemyControllerSM stateMachine) : base("EnemyIdle", stateMachine)
        {
            _enemyControllerSm = (EnemyControllerSM)stateMachine;
        }

        public override void Enter()
        {
            _enemyControllerSm.animator.SetBool("Move", false);
            _enemyControllerSm.animator.SetBool("Fight", false);
        }

        public override void UpdateLogic()
        {
            

        }

        public override void UpdatePhysics()
        {
            for (int i = 0; i < EnemyController.instance.playerList.Count; i++)
            {
                float distance = Vector3.Distance(EnemyController.instance.playerList[i].position,
                    _enemyControllerSm.thisTransform.position);
                if (distance < _enemyControllerSm.radiusLoock)
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
                else if (distance > 15f)
                {
                    _enemyControllerSm.meshRenderer.enabled = false;
                }
                else
                {
                    _enemyControllerSm.meshRenderer.enabled = true;
                }
            }
        }

        
    }
}