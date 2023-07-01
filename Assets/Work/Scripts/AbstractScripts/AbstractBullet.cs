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
    public TrailRenderer trailRenderer;
    public LayerMask damageMask;
    public BulletController bulletController;
}
