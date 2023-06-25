using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletController : NetworkBehaviour
{
    [SerializeField] private AbstractBullet _bullet;
    private List<AbstractBullet> _bullets = new List<AbstractBullet>();
    private List<AbstractBullet> _liveBullet = new();
    private float _distance;
    public static BulletController instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!isServer) return;
        for (int i = 0; i < 30; i++)
        {
            var bul = Instantiate(_bullet, this.transform);
            _bullets.Add(bul);
            NetworkServer.Spawn(bul.gameObject);
            bul.gameObject.SetActive(false);
        }
    }

    public void AddBullet(AbstractBullet bullet, Transform firePoint, float scram)
    {
        for (int i =0; i <= _bullets.Count;i++)
        {
            if (!_bullets[i].gameObject.activeSelf)
            {
                _bullets[i].speed = bullet.speed;
                _bullets[i].damage = bullet.damage;
                _bullets[i].thisTR.position = firePoint.position;
                _bullets[i].thisTR.forward = firePoint.forward + new Vector3 (Random.Range(-1*scram,scram), 0, Random.Range(-1*scram,scram));
                _bullets[i].previousePosition = firePoint.position;
                _liveBullet.Add(_bullets[i]);
                _bullets[i].gameObject.SetActive(true);
                break;
            }

            if (i == _bullets.Count)
            {
                var bulls = Instantiate(_bullet, firePoint.position, quaternion.identity,this.transform);
                _bullets.Add(bulls);
                NetworkServer.Spawn(bulls.gameObject);
                bulls.speed = bullet.speed;
                bulls.damage = bullet.damage;
                bulls.thisTR.position = firePoint.position;
                bulls.thisTR.forward = firePoint.forward + new Vector3 (Random.Range(-1*scram,scram), 0, Random.Range(-1*scram,scram));;
                bulls.previousePosition = firePoint.position;
                _liveBullet.Add(bulls);
            }
        }  
    }

    public void DestroyBullet(AbstractBullet bullet)
    {
        bullet.transform.gameObject.SetActive(false);
        _liveBullet.Remove(bullet);
        bullet.thisTR.position = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!isServer) return;
        if (_liveBullet.Count > 0)
        {
            foreach (var bullet in _liveBullet)
            {
                bullet.previousePosition = bullet.thisTR.position;
                bullet.thisTR.position += bullet.thisTR.forward * bullet.speed * Time.deltaTime;
            }

            for (int i=0; i < _liveBullet.Count;i++)
            {
                _distance = Vector3.Distance(_liveBullet[i].thisTR.position, _liveBullet[i].previousePosition);
                RaycastHit hit;
                AbstractDamageCollider damageController;
                if (Physics.Raycast(_liveBullet[i].thisTR.position, -1 * _liveBullet[i].thisTR.forward, out hit, _distance,
                        _liveBullet[i].damageMask))
                {
                    if (hit.transform.gameObject.TryGetComponent(out damageController))
                    damageController.TakeDamage(_liveBullet[i].damage,hit.point,_liveBullet[i].thisTR.forward);
                    Debug.Log(damageController);
                    DestroyBullet(_liveBullet[i]);
                    i--;
                }
            }
        }
    }
}