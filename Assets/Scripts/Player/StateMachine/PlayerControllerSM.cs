using System.Collections;
using Player.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;

namespace Player.StateMachine
{
    public class PlayerControllerSM : PlayerStateMachine
    {
        public float nonGunSpeed, gunSpeed;
        public Joystick movementJoyStick;
        public Joystick fireJoyStick;
        


        [HideInInspector] public PlayerNonGunState nonGunState;
        [HideInInspector] public PlayerGunState gunState;
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Transform thisTransform;

        [SerializeField] private AbstractGunScripts _gun;
        [SerializeField] private Transform _gunInHandTransform;

        private LayerMask _stage;
        private Button _use;
        private AbstractGunScripts _firstGun;

        private void Start()
        
        {
            if (!isLocalPlayer) return;
            nonGunState = new PlayerNonGunState(this);
            gunState = new PlayerGunState(this);
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            thisTransform = GetComponent<Transform>();
            Camera.main.GetComponent<CameraController>().player = thisTransform;
            movementJoyStick = CanvasController.instance.move;
            fireJoyStick = CanvasController.instance.fire;
            _use = CanvasController.instance.useButton;
            _use.onClick.AddListener(Use);
            ChangeState(nonGunState);
            _firstGun = Instantiate(_gun, _gunInHandTransform);
            animator.runtimeAnimatorController = _firstGun._gunController;
        }

        

        public bool IsWallOnWay(Vector3 direction)
        {
            return Physics.Raycast(thisTransform.position + Vector3.up, direction, 0.5f, _stage);
        }

        public void Fire()
        {
            _firstGun.Fire();
        }
        
        

        protected override PlayerBaseState GetInitialState()
        {
            return nonGunState;
        }

        private void Use()
        {
            animator.SetBool("Use", true);
            movementJoyStick.gameObject.SetActive(false);
            fireJoyStick.gameObject.SetActive(false);
            StartCoroutine(EndUse());
        }

        IEnumerator  EndUse()
        {
            yield return new WaitForSeconds(3f);
            movementJoyStick.gameObject.SetActive(true);
            fireJoyStick.gameObject.SetActive(true);
            animator.SetBool("Use", false);
        }
    }
}