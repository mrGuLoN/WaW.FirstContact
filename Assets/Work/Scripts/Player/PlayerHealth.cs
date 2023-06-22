using Mirror;
using Player.StateMachine;
using UnityEngine;

public class PlayerHealth : AbstractHealth
{
    [SerializeField]
    private float _startHealth;

    [SerializeField] private float _currentHealth;
    [SerializeField] private GameObject _blood;

    private Animator _animator;
    private Transform _thisTransform;
    private Vector3 _directionDamage;
    private PlayerControllerSM _playerController;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerControllerSM>();
        _animator = GetComponent<Animator>();
        _thisTransform = GetComponent<Transform>();
        _currentHealth = _startHealth;
    }

    public override void TakeDamage(float damage, Vector3 point, Vector3 direction, Rigidbody rb)
    {
        _currentHealth -= damage;
        var blood = Instantiate(_blood, point,Quaternion.Euler(direction));
        blood.transform.forward = direction;
        _directionDamage = (point - _thisTransform.position).normalized;
        NetworkServer.Spawn(blood);
        if (_currentHealth <= 0)
        {
            _playerController.enabled = false;
            _animator.enabled = false;
            base.RagdollOn();
           //_animator.SetTrigger("Dead");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
