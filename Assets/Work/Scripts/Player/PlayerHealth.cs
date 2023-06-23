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
    private CharacterController _characterController;
    private Vector3 _directionDamage;
    private PlayerControllerSM _playerController;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (!isLocalPlayer) return;
        _characterController = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerControllerSM>();
        _animator = GetComponent<Animator>();
        _thisTransform = GetComponent<Transform>();
        _currentHealth = _startHealth;
    }

    public override void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        _currentHealth -= damage;
        var blood = Instantiate(_blood, point,Quaternion.Euler(direction));
        blood.transform.forward = direction;
        NetworkServer.Spawn(blood);
        direction = _thisTransform.TransformDirection(direction).normalized;
        _animator.SetFloat("DamageX", direction.x);
        _animator.SetFloat("DamageY", direction.z);
        _animator.SetTrigger("Damage");
        
        if (_currentHealth <= 0)
        {
            _playerController.enabled = false;
            _characterController.enabled = false;
            _animator.SetTrigger("Dead");
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
