using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class AbstractHealth : NetworkBehaviour

{
    private List<Collider> _colliders;
    private Transform _centerTransform;
    
    public virtual void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>().ToList();
        foreach (var rigidbody in _colliders)
        {
            var abstractDamageCollider = rigidbody.transform.gameObject.AddComponent<AbstractDamageCollider>();
            abstractDamageCollider.abstractHealth = this;
        }
        ChangeLayerDamage();
        this.transform.gameObject.layer = 7;
    }
    public virtual void ChangeLayerDamage()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = true;
            collider.transform.gameObject.layer = 8;
        }
    }

    public virtual void OffDamageCollider()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
    }

    public virtual void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
