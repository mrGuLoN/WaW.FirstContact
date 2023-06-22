using Mirror;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbstractGunScripts : NetworkBehaviour
{
    [SerializeField] protected ParticleSystem _splash;
    [SerializeField] protected AnimatorOverrideController _gunController;
    [SerializeField] protected AbstractBullet _bullet;
    [SerializeField] private protected int _pelvis;
    [SerializeField] private protected float _damage;
    [SerializeField] private protected float _speed;
    [SerializeField] private protected float _scram;
    [SerializeField] private protected Transform _firePoint;


    public virtual void CmdFire()
    {
        Debug.Log("FIRE");
        for (int i = 0; i < _pelvis; i++)
        {
           SpawnBullet();
        }
    }

    [Command]
    private void SpawnBullet()
    {
        var bullet = Instantiate(_bullet, _firePoint.position, quaternion.identity);
        bullet.transform.forward = _firePoint.forward +
                                   new Vector3(Random.Range(-1 * _scram, _scram), 0,
                                       Random.Range(-1 * _scram, _scram));
        bullet.speed = _speed;
        bullet.damage = _damage;
        NetworkServer.Spawn(bullet.gameObject);
    }
}