using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbstractHealth : MonoBehaviour
{
    [SerializeField] private LayerMask _damageLayer, _deadLayer;
   
    private List<Rigidbody> _allRigidbody;
    private Transform _centerTransform;
    
    public virtual void Awake()
    {
        _allRigidbody = GetComponentsInChildren<Rigidbody>().ToList();
        foreach (var rigidbody in _allRigidbody)
        {
            var abstractDamageCollider = rigidbody.transform.gameObject.AddComponent<AbstractDamageCollider>();
            abstractDamageCollider.abstractHealth = this;
        }
        RagdollOff();
    }

    public virtual void RagdollOn()
    {
        foreach (var rigidbody in _allRigidbody)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            rigidbody.transform.gameObject.layer = _deadLayer;
        }
    }
    
    public virtual void RagdollOff()
    {
        foreach (var rigidbody in _allRigidbody)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            rigidbody.transform.gameObject.layer = _damageLayer;
        }
    }

    public virtual void TakeDamage(float damage, Vector3 point, Vector3 direction, Rigidbody rb)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
