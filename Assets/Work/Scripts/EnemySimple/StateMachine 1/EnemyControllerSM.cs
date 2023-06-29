using Enemy.StateMachine.States;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy.StateMachine
{
    public class EnemyControllerSM : EnemyStateMachine
    {
        public float walkSpeed, runSpeed, radiusLoock, damage;
        public LayerMask stage;
        public SkinnedMeshRenderer meshRenderer;

        [HideInInspector] public EnemyIdle idle;
        [HideInInspector] public EnemyAttack attack;
        [HideInInspector] public EnemyFindObject enemyFindObject;
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Transform thisTransform;
        [HideInInspector] public Transform target;
        [HideInInspector] public float speedMove, healthBank, speedAnimation;
        [HideInInspector] public Vector3 playerLastCoordinate;


        private void Awake()
        {
            EnemyController.instance.AddEnemy(this);
        }

        private void Start()

        {
            idle = new EnemyIdle(this);
            attack = new EnemyAttack(this);
            enemyFindObject = new EnemyFindObject(this);
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            thisTransform = GetComponent<Transform>();
            speedAnimation = Random.Range(0.95f, 1.05f);
            animator.SetFloat("SpeedMovement", speedAnimation);
            ChangeState(idle);
        }

        public void CheckDistanse()
        {
            for (int i = 0; i < EnemyController.instance.playerList.Count; i++)
            {
                if (Vector3.Distance(thisTransform.position,
                        EnemyController.instance.playerList[i].transform.position) > 15)
                {
                    meshRenderer.enabled = false;
                }
                else
                {
                    meshRenderer.enabled = true;
                }
            }
        }


        public bool IsWallOnWay(Vector3 direction)
        {
            return Physics.Raycast(thisTransform.position + Vector3.up, direction, 0.5f, stage);
        }

        public void Hit()
        {
            if (isServer)
            target.gameObject.GetComponent<AbstractHealth>().TakeDamage(damage, target.position + Vector3.up*1.6f, thisTransform.forward);
        }


        protected override EnemyBaseState GetInitialState()
        {
            return idle;
        }
    }
}