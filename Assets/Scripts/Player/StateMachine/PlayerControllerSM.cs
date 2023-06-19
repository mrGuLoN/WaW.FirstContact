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
        public AbstractGunScripts firstGun;

        [SerializeField] private LayerMask _stage;
        [SerializeField] private Button _use; 
        

        [HideInInspector] public PlayerNonGunState nonGunState;
        [HideInInspector] public PlayerGunState gunState;
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Transform thisTransform;
        

        

        private void Awake()
        
        {
            if (!isLocalPlayer) return;
            nonGunState = new PlayerNonGunState(this);
            gunState = new PlayerGunState(this);
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            thisTransform = GetComponent<Transform>();
        }

        private void Start()
        {
            if (!isLocalPlayer) return;
            ChangeState(nonGunState);
            _use.onClick.AddListener(Use);
            Camera.main.GetComponent<CameraController>().player = thisTransform;
        }

        public bool IsWallOnWay(Vector3 direction)
        {
            return Physics.Raycast(thisTransform.position + Vector3.up, direction, 0.5f, _stage);
        }

        public void Fire()
        {
            firstGun.Fire();
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