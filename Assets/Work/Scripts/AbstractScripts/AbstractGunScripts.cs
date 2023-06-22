using Mirror;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbstractGunScripts : NetworkBehaviour
{
    [SerializeField] protected ParticleSystem _splash;
    [SerializeField] protected AnimatorOverrideController _gunController;
    [SerializeField] private protected int _pelvis;
    [SerializeField] private protected float _damage;
    [SerializeField] private protected float _speed;
    [SerializeField] private protected float _scram;
    [SerializeField] private protected Transform _firePoint;
    
    [SerializeField] private AbstractBullet _bullet = new AbstractBullet();


    public virtual void CmdFire()
    {
        for (int i = 0; i < _pelvis; i++)
        {
           SpawnBullet();
        }
    }

    
    private void SpawnBullet()
    {
        _bullet.damage = _damage;
        _bullet.speed = _speed;
        BulletController.instance.AddBullet(_bullet, _firePoint, _scram);
    }
}