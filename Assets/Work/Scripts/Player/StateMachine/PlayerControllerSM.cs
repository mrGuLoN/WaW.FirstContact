using System;
using System.Collections;
using Mirror;
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
        [SerializeField] private Transform _rightHandGunPoint;


        [HideInInspector] public PlayerNonGunState nonGunState;
        [HideInInspector] public PlayerGunState gunState;
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Transform thisTransform;

        private void Awake()
        {
            CmdRespawnGun();
        }

        private void Start()

        {
            if (!isLocalPlayer) return;
            nonGunState = new PlayerNonGunState(this);
            gunState = new PlayerGunState(this);
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            thisTransform = GetComponent<Transform>();
            EnemyController.instance.AddPlayer(thisTransform);
            Camera.main.GetComponent<CameraController>().player = thisTransform;
            movementJoyStick = CanvasController.instance.move;
            fireJoyStick = CanvasController.instance.fire;
            _use = CanvasController.instance.useButton;
            _use.onClick.AddListener(Use);
            ChangeState(nonGunState);
        }

        private void OnDestroy()
        {
            EnemyController.instance.RemovePlayer(thisTransform);
        }

        private void CmdRespawnGun()
        {
            firstGun = Instantiate(firstGun, _rightHandGunPoint.position, Quaternion.identity, _rightHandGunPoint);
            NetworkServer.Spawn(firstGun.gameObject);
        }


        public bool IsWallOnWay(Vector3 direction)
        {
            return Physics.Raycast(thisTransform.position + Vector3.up, direction, 0.5f, _stage);
        }

        
        public void CmdFire()
        {
            firstGun.CmdFire();
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

        IEnumerator EndUse()
        {
            yield return new WaitForSeconds(3f);
            movementJoyStick.gameObject.SetActive(true);
            fireJoyStick.gameObject.SetActive(true);
            animator.SetBool("Use", false);
        }
    }
}