using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletController : NetworkBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _bulletParrent;
    [SerializeField] private List<AbstractBullet> _bullets = new List<AbstractBullet>();
    [SerializeField] private List<AbstractBullet> _liveBullet = new();
    private float _distance;
    public static BulletController instance = null;
    

    [Command]
    public void Start()
    {
        if (!isLocalPlayer) return;
        for (int i = 20; i > 0; i--)
        {
            var buben = Instantiate(_bullet, null);
            buben.GetComponent<AbstractBullet>().bulletController = this;
            _bullets.Add(buben.GetComponent<AbstractBullet>());
           // NetworkServer.Spawn(buben);
        }
    }
    public void AddBullet(float damage, float speed, Transform firePoint, float scram)
    {
        CmdAddBullet(damage, speed, firePoint, scram);
    }

   
    public void CmdAddBullet(float damage, float speed, Transform firePoint, float scram)
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (_bullets[i].gameObject.activeSelf == false)
            {
                _bullets[i].gameObject.SetActive(true);
                _bullets[i].speed = speed;
                _bullets[i].damage = damage;
                _bullets[i].thisTR.position = firePoint.position;
                _bullets[i].thisTR.forward = firePoint.forward +
                                             new Vector3(Random.Range(-1 * scram, scram), 0,
                                                 Random.Range(-1 * scram, scram));
                _bullets[i].previousePosition = firePoint.position;
                _liveBullet.Add(_bullets[i]);
                RpcAddBullet(_bullets[i]);
                return;
            }
        }

        var bulls = Instantiate(_bullet, firePoint.position, Quaternion.identity, this.transform);
        var bullsAbstract = bulls.GetComponent<AbstractBullet>();
        _bullets.Add(bullsAbstract);
        NetworkServer.Spawn(bulls);
        bullsAbstract.speed = speed;
        bullsAbstract.damage = damage;
        bullsAbstract.bulletController = this;
        bullsAbstract.thisTR.position = firePoint.position;
        bullsAbstract.thisTR.forward = firePoint.forward +
                                       new Vector3(Random.Range(-1 * scram, scram), 0, Random.Range(-1 * scram, scram));
        bullsAbstract.previousePosition = firePoint.position;
        _liveBullet.Add(bullsAbstract);
        _bullets.Add(bullsAbstract);
        RpcAddBullet(bullsAbstract);
    }

    
    public void RpcAddBullet(AbstractBullet gameObject)
    {
        gameObject.trailRenderer.enabled = true;
        gameObject.gameObject.SetActive(true);
    }

    public void DestroyBullet(AbstractBullet bullet)
    {
        CmdDestroyBullet(bullet);
    }
    [Command]
    public void CmdDestroyBullet(AbstractBullet bullet)
    {
        _liveBullet.Remove(bullet);
        bullet.gameObject.SetActive(false);
        bullet.thisTR.position = transform.position;
        RpcDestroyBullet(bullet);
    }

    [ClientRpc]
    private void RpcDestroyBullet(AbstractBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.thisTR.position = transform.position;
    }
    
    [Command]
    public void UpdateBulletPosition()
    {
        if (!isServer) return;
        for (int i = 0; i < _bullets.Count; i++)
        {
            _bullets[i].previousePosition = _bullets[i].thisTR.position;
            _bullets[i].thisTR.position += _bullets[i].thisTR.forward * _bullets[i].speed * Time.deltaTime;
        }

        for (int i = 0; i < _liveBullet.Count; i++)
        {
            var newPosition = _liveBullet[i].thisTR.position;
            _distance = Vector3.Distance(_liveBullet[i].thisTR.position, _liveBullet[i].previousePosition);
            RaycastHit hit;
            AbstractDamageCollider damageController;
            if (Physics.Raycast(_liveBullet[i].thisTR.position, -1 * _liveBullet[i].thisTR.forward, out hit, _distance,
                    _liveBullet[i].damageMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out damageController))
                    damageController.TakeDamage(_liveBullet[i].damage,hit.point,_liveBullet[i].thisTR.forward);
                RpcBulletMove( _liveBullet[i].thisTR, hit.point);
                CmdDestroyBullet(_liveBullet[i]);
                i--;
            }
            else
            {
                RpcBulletMove( _liveBullet[i].thisTR, newPosition);
            }
        }
    }
    
    [ClientRpc]
    private void RpcBulletMove(Transform thisTR, Vector3 newPosition)
    {
        thisTR.position = newPosition;
    }
}