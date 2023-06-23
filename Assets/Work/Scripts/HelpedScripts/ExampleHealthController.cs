using UnityEngine;

public class ExampleHealthController : AbstractHealth
{
   
    [SerializeField]
    private float _startHealth;

    [SerializeField] private float _currentHealth;
    [SerializeField] private GameObject _blood;

    private Animator _animator;
    private Transform _thisTransform;
    private CharacterController _characterController;
    private Vector3 _directionDamage;

   

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _thisTransform = GetComponent<Transform>();
        _currentHealth = _startHealth;
    }

    public override void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        _currentHealth -= damage;
        Debug.Log("HealthController");
        var blood = Instantiate(_blood, point,Quaternion.Euler(direction));
        blood.transform.forward = direction;
       
        direction = _thisTransform.TransformDirection(direction).normalized;
        _animator.SetFloat("DamageX", direction.x);
        _animator.SetFloat("DamageY", direction.z);
        _animator.SetTrigger("Damage");
        
        if (_currentHealth <= 0)
        {
            _characterController.enabled = false;
            _animator.SetTrigger("Dead");
        }
    }
}
