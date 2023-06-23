using System;
using System.Collections;
using Enemy.StateMachine;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealth : AbstractHealth
{
    // Start is called before the first frame update
    [SerializeField] private float _health;
    [SerializeField] private GameObject _blood;
    [SerializeField]private Animator _animator;
    [SerializeField] private GameObject _visual;
    [SerializeField]private Transform _thisTransform;
    
    
    private float _currentHealth;
    private float _runHealth;
    private float _speedMove;
    private EnemyControllerSM _enemyControllerSM;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        _enemyControllerSM = GetComponent<EnemyControllerSM>();
        _currentHealth = _health;
        _runHealth = 0.5f * _health;
        _animator = GetComponent<Animator>();
        _thisTransform = GetComponent<Transform>();
    }
    
    public override void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        _currentHealth -= damage;
        _speedMove = (_health - _currentHealth) / _runHealth;
        _enemyControllerSM.speedMove = _speedMove;
        _animator.SetFloat("Health", _speedMove);
        var blood = Instantiate(_blood, point,Quaternion.Euler(direction));
        blood.transform.forward = direction;
        NetworkServer.Spawn(blood);
        _animator.SetTrigger("Damage");
        
        if (_currentHealth <= 0)
        {
            _animator.SetTrigger("Dead");
            StartCoroutine(SetStatic());
        }
    }

    IEnumerator SetStatic()
    {
        yield return new WaitForSeconds(10f);
        gameObject.isStatic = true;
        _visual.isStatic = true;
    }

    
   
}
