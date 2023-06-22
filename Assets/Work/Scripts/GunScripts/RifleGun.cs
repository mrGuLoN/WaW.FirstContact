using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RifleGun : AbstractGunScripts
{
    
    public override void CmdFire()
    {
        _splash.Play();
        Debug.Log("FIRE");
        for (int i = 0; i < _pelvis; i++)
        {
            CmdSpawnBullet();
        }
    }
    
    
    private void CmdSpawnBullet()
    {
        var bullet = Instantiate(_bullet, _firePoint.position, Quaternion.identity);
        bullet.transform.forward = _firePoint.forward +
                                   new Vector3(Random.Range(-1 * _scram, _scram), 0,
                                       Random.Range(-1 * _scram, _scram));
        bullet.speed = _speed;
        bullet.damage = _damage;
        NetworkServer.Spawn(bullet.gameObject);
    }
}
