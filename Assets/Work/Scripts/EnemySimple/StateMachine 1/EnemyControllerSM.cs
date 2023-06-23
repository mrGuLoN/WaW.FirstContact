using System;
using System.Collections;
using Enemy.StateMachine.States;
using Mirror;
using Player.StateMachine.States;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enemy.StateMachine
{
    public class EnemyControllerSM : EnemyStateMachine
    {
        public float walkSpeed, runSpeed, radiusLoock;
        public LayerMask stage;
        
        [HideInInspector] public EnemyIdle idle;
        [HideInInspector] public EnemyAttack attack;
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Transform thisTransform;
        [HideInInspector] public Transform target;
        [HideInInspector] public float speedMove, healthBank, speedAnimation;

       

        private void Awake()
        {
            
        }

        private void Start()

        {
            if (!isServer) return;
            idle = new EnemyIdle(this);
            attack = new EnemyAttack(this);
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            thisTransform = GetComponent<Transform>();
            speedAnimation = Random.Range(0.95f, 1.05f);
            animator.SetFloat("SpeedMovement", speedAnimation);
            ChangeState(idle);
        }


        public bool IsWallOnWay(Vector3 direction)
        {
            return Physics.Raycast(thisTransform.position + Vector3.up, direction, 0.5f, stage);
        }


        protected override EnemyBaseState GetInitialState()
        {
            return idle;
        }
    }
}