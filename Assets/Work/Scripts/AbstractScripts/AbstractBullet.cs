using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class AbstractBullet : NetworkBehaviour
{
    public float damage;
    public float speed;
    public Transform thisTR;
    public Vector3 previousePosition;
    public GameObject trailRenderer;
    public LayerMask damageMask;
    public BulletController bulletController;

    private void Start()
    {
        thisTR = GetComponent<Transform>();
        TrailOff();
    }
   
    public virtual void TrailOn()
    {
        trailRenderer.SetActive(true);
    }
    public virtual void TrailOff()
    {
        trailRenderer.SetActive(false);

    }
}
