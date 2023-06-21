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

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
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
        if (_currentHealth <= 0)
        {
            base.RagdollOn();
            rb.AddForce(direction * damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
