using Unity.VisualScripting;
using UnityEngine;

namespace Player.StateMachine.States
{
    public class PlayerGunState: PlayerBaseState
    {
        private float _speed, _imputMagnitude;
        private Vector3 _loocVector, _movementVector;
        
        private PlayerControllerSM _playerControllerSm;
        private Vector3 _targetPoint, _directional;
        private BulletController _bulletController;
        

        public PlayerGunState(PlayerControllerSM _stateMachine) : base("PlayerPistolState", _stateMachine)
        {
            _playerControllerSm = (PlayerControllerSM)stateMachine;
        }

        public override void Enter()
        {
            if (_playerControllerSm.secondFire.isOn)
            {
                _playerControllerSm.animator.SetBool("SecondWeaponFire", true);
                _playerControllerSm.animator.SetBool("Fire", false);
            }
            else
            {
                _playerControllerSm.animator.SetBool("SecondWeaponFire", false);
                _playerControllerSm.animator.SetBool("Fire", true);
            }

            _speed = _playerControllerSm.gunSpeed;
        }

        public override void UpdateLogic()
        {
           
        }

        public override void UpdatePhysics()
        {
            _playerControllerSm.bulletController.UpdateBulletPosition();
            _loocVector = new Vector3(_playerControllerSm.fireJoyStick.Horizontal, 0,
                _playerControllerSm.fireJoyStick.Vertical);
            _movementVector = new Vector3(_playerControllerSm.movementJoyStick.Horizontal, 0,
                _playerControllerSm.movementJoyStick.Vertical);
            if (_loocVector != Vector3.zero)
            {
                _playerControllerSm.thisTransform.forward = _loocVector;
            }
            else
            {
                _playerControllerSm.ChangeState(_playerControllerSm.nonGunState);
            }

            if (_movementVector != Vector3.zero || _playerControllerSm.IsWallOnWay(_movementVector))
            {
                _playerControllerSm.thisTransform.position += _movementVector.normalized * _speed * Time.deltaTime;
                _playerControllerSm.characterController.Move(_movementVector.normalized * _speed * Time.deltaTime  + Vector3.down*10f);
                _movementVector = _playerControllerSm.thisTransform.InverseTransformDirection(_movementVector);
                _playerControllerSm.animator.SetFloat("InputX", _movementVector.x);
                _playerControllerSm.animator.SetFloat("InputY", _movementVector.z);
            }
            else
            {
                _playerControllerSm.animator.SetFloat("InputX", 0);
                _playerControllerSm.animator.SetFloat("InputY", 0);
            }
            
        }
    }
}