using Mirror;
using Player.StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : AbstractHealth
{
    [SerializeField] private float _startHealth;

    [SerializeField] private float _currentHealth;
    [SerializeField] private GameObject _blood;

    private Animator _animator;
    private Transform _thisTransform;
    private CharacterController _characterController;
    private Vector3 _directionDamage;
    private PlayerControllerSM _playerController;
    private Text _health;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (!isLocalPlayer) return;
        _health = CanvasController.instance.health;
        _characterController = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerControllerSM>();
        _animator = GetComponent<Animator>();
        _thisTransform = GetComponent<Transform>();
        _currentHealth = _startHealth;
        _health.text = _currentHealth.ToString();
    }

    //[ClientRpc]
    public override void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        if (!isLocalPlayer) return;
        if (_currentHealth < 0) return;
        _currentHealth -= damage;
        var blood = Instantiate(_blood, point, Quaternion.Euler(direction));
        blood.transform.forward = direction;
        NetworkServer.Spawn(blood);
        direction = -1*_thisTransform.TransformDirection(direction).normalized;
        _animator.SetFloat("DamageX", direction.x);
        _animator.SetFloat("DamageY", direction.z);
        _animator.SetTrigger("Damage");
        _health.text = _currentHealth.ToString();
        if (_currentHealth <= 0)
        {
            _health.text = "Nice meat";
            _playerController.enabled = false;
            _characterController.enabled = false;
            _animator.SetTrigger("Dead");
            _animator.SetBool("Fire", false);
            EnemyController.instance.RemovePlayer(_thisTransform);
        }
    }

   

    // Update is called once per frame
    void Update()
    {
    }
}