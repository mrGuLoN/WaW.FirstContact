using DG.Tweening;
using UnityEngine;

namespace Player.StateMachine.States
{
    public class PlayerNonGunState : PlayerBaseState
    {
        private float _speed, _imputMagnitude;
        private Vector3 _movementVector;
        private PlayerControllerSM _playerControllerSm;

        public PlayerNonGunState(PlayerControllerSM stateMachine) : base("PlayerNonGunState", stateMachine)
        {
            _playerControllerSm = (PlayerControllerSM)stateMachine;
        }

        public override void Enter()
        {
            _speed = _playerControllerSm.nonGunSpeed;
            _playerControllerSm.animator.SetBool("Fire", false);
            _playerControllerSm.animator.SetBool("SecondWeaponFire", false);
        }

        public override void UpdateLogic()
        {
            

        }

        public override void UpdatePhysics()
        {
            _playerControllerSm.bulletController.UpdateBulletPosition();
            _movementVector = new Vector3(_playerControllerSm.movementJoyStick.Horizontal, 0,
                _playerControllerSm.movementJoyStick.Vertical);
            if (_playerControllerSm.fireJoyStick.Horizontal!=0 ||_playerControllerSm.fireJoyStick.Horizontal!=0)
                _playerControllerSm.ChangeState(_playerControllerSm.gunState);
            
            if (_movementVector != Vector3.zero && !_playerControllerSm.IsWallOnWay(_movementVector))
            {
                _playerControllerSm.thisTransform.forward = _movementVector.normalized;
                _playerControllerSm.characterController.Move(_movementVector.normalized * _speed * Time.deltaTime+ Vector3.down*10f);
                _playerControllerSm.animator.SetFloat("InputY",1);
            }
            else
            {
                _playerControllerSm.animator.SetFloat("InputY",0);
            }
        }

        
    }
}