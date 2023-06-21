using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class AbstractGunScripts : NetworkBehaviour
{
    [SerializeField] public ParticleSystem _splash;
    [SerializeField] public AnimatorOverrideController _gunController;
    [SerializeField] public int _bulletInMagazine, _allBullet, _pellets;
    [SerializeField] public AbstractBullet _bullet;
    [SerializeField] public float _scatterX, _scatterY;
    [SerializeField] public Transform _firepointTransform;
    public virtual void Fire()
    {
        
    }

    
    public virtual void RespawnBullet(Transform firePoint)
    {
        for (int i = _pellets; i > 0; i--)
        {
            var bullet = Instantiate(_bullet, firePoint.position, quaternion.identity);
            bullet.transform.forward = firePoint.forward + firePoint.InverseTransformDirection(new Vector3(_scatterX,_scatterY,0));
            NetworkServer.Spawn(bullet.gameObject);
            ServerBulletController.instance.AddBullet(bullet);
        }

        _bulletInMagazine--;
    }
}
