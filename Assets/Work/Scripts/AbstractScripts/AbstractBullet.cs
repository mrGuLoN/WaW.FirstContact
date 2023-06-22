using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AbstractBullet : NetworkBehaviour
{
    public float damage;
    public float speed;
    public Transform _thisTR;
    public Vector3 previousePosition;
    public LayerMask damageMask;

    public virtual void Awake()
    {
        _thisTR = GetComponent<Transform>();
        BulletController.instance.AddBullet(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
