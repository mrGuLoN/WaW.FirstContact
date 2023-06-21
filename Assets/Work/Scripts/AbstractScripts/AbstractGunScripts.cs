using Mirror;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbstractGunScripts : NetworkBehaviour
{
    [SerializeField] protected ParticleSystem _splash;
    [SerializeField] protected AnimatorOverrideController _gunController;
    [SerializeField] protected AbstractBullet _bullet;
    [SerializeField] private int _pelvis;
    [SerializeField] private float _damage, _speed, _scram;
    [SerializeField] private Transform _firePoint;
    
    [Command]
    public virtual void Fire()
    {
        for (int i = 0; i < _pelvis; i++)
        {
            var bullet = Instantiate(_bullet, _firePoint.position, quaternion.identity);
            bullet.transform.forward = _firePoint.forward + new Vector3(Random.Range(-1*_scram, _scram),0,Random.Range(-1*_scram, _scram));
            bullet.speed = _speed;
            bullet.damage = _damage;
            NetworkServer.Spawn(bullet.gameObject);
        }
    }
}
