using System;
using System.Collections;
using Enemy.StateMachine;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealth : AbstractHealth
{
    [SerializeField] private float _health;
    [SerializeField] private GameObject _blood;
    [SerializeField]private Animator _animator;
    [SerializeField] private GameObject _visual;
    [SerializeField]private Transform _thisTransform;
    
    
    private float _currentHealth;
    private float _runHealth;
    private float _speedMove;
    private EnemyControllerSM _enemyControllerSM;
    private CharacterController _character;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        _character = GetComponent<CharacterController>();
        _enemyControllerSM = GetComponent<EnemyControllerSM>();
        _currentHealth = _health;
        _runHealth = 0.5f * _health;
        _animator = GetComponent<Animator>();
        _thisTransform = GetComponent<Transform>();
    }

    public override void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        CMDDamageEnemy(damage, point, direction);
    }

    [Server]

    public void CMDDamageEnemy(float damage, Vector3 point, Vector3 direction)
    {
        Debug.Log("Bububu");
        _currentHealth -= damage;
        _speedMove = (_health - _currentHealth) / _runHealth;
        _enemyControllerSM.speedMove = _speedMove;
        _animator.SetFloat("Health", _speedMove);
        _animator.SetTrigger("Damage");
        RpcDamageVisual( point,  direction);
        if (_currentHealth <= 0)
        {
            EnemyController.instance.RemoveEnemy(_enemyControllerSM);
            _character.enabled = false;
            _animator.SetTrigger("Dead");
            OffDamageCollider();
            RpcDeadVisual();
            StartCoroutine(SetStatic());
        }
    }

    [ClientRpc]
    private void RpcDamageVisual(Vector3 point,Vector3 direction)
    {
        _animator.SetFloat("Health", _speedMove);
        var blood = Instantiate(_blood, point,Quaternion.Euler(direction), _thisTransform);
        blood.transform.forward = direction;
        _animator.SetTrigger("Damage");
    }

    [ClientRpc]
    private void RpcDeadVisual()
    {
        _character.enabled = false;
        _animator.SetTrigger("Dead");
        OffDamageCollider();
    }


    IEnumerator SetStatic()
    {
        yield return new WaitForSeconds(10f);
        gameObject.isStatic = true;
        _visual.isStatic = true;
        RpcSetStatic();
    }

    [ClientRpc]
private void RpcSetStatic()
    {
        gameObject.isStatic = true;
        _visual.isStatic = true;
    }

    
   
}
