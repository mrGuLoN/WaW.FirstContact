using System;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AbstractGunScripts : NetworkBehaviour
{
    public Animator animator;
    
    [SerializeField] protected ParticleSystem _splash;
    [SerializeField] protected AnimatorOverrideController _gunController;
    [SerializeField] private protected int _pelvis;
    [SerializeField] private protected float _damage;
    [SerializeField] private protected float _speed;
    [SerializeField] private protected float _scram;
    [SerializeField] private protected Transform _firePoint;
    [SerializeField] public int magazine;
    [SerializeField] public int allAmmo;
    [SerializeField] private AbstractBullet _bullet = new AbstractBullet();

    private List<AbstractBullet> _bullets = new();
    private List<AbstractBullet> _liveBullet = new();
    private Text _ammo;
    private int _currentMagazine;
    private float _distance;
    public void Start()
    {
        _ammo = CanvasController.instance.ammo;
        _currentMagazine = magazine;
        _ammo.text = _currentMagazine + " / " + allAmmo;
        for (int i = 0; i < _pelvis*10f; i++)
        {
            var bul = Instantiate(_bullet, BulletController.instance.transform);
            _bullets.Add(bul);
            NetworkServer.Spawn(bul.gameObject);
            bul.gameObject.SetActive(false);
        }
    }
    

    public virtual void SetAnimator(Animator Animator)
    {
        animator = Animator;
    }

    [ClientRpc]
    public void UpAmmo(int magazineUp)
    {
        allAmmo = this.magazine * magazineUp;
        _ammo.text = _currentMagazine + " / " + allAmmo;
        
    }

    public virtual void CmdFire()
    {
        if (_currentMagazine <= 0)
        {
            animator.SetBool("Reload", true);
            return;
        }
        _currentMagazine--;
        _ammo.text = _currentMagazine + " / " + allAmmo;
        for (int i = 0; i < _pelvis; i++)
        {
           SpawnBullet();
        }
    }

    public void Reloading()
    {
        animator.SetBool("Reload", false);
        if (allAmmo > magazine)
        {
            _currentMagazine = magazine;
            allAmmo -= magazine;
            _ammo.text = _currentMagazine + " / " + allAmmo;
        }
        else if (allAmmo <= 0)
        {
            _currentMagazine = 1;
            _ammo.text = _currentMagazine + " / " + "Damn!!!";
        }
        else
        {
            _currentMagazine = allAmmo;
            allAmmo = 0;
            _ammo.text = _currentMagazine + " / " + "Damn!!!";
        }
    }


    public void SpawnBullet()
    {
        for (int i =0; i <= _bullets.Count;i++)
        {
            Debug.Log("FFFFirrreeee!!!!");
            if (!_bullets[i].gameObject.activeSelf)
            {
                _bullets[i].speed = _speed;
                _bullets[i].damage = _damage;
                _bullets[i].thisTR.position = _firePoint.position;
                _bullets[i].thisTR.forward = _firePoint.forward + new Vector3 (Random.Range(-1*_scram,_scram), 0, Random.Range(-1*_scram,_scram));
                _bullets[i].previousePosition = _firePoint.position;
                _liveBullet.Add(_bullets[i]);
                _bullets[i].gameObject.SetActive(true);
                break;
            }

            if (i == _bullets.Count)
            {
                var bulls = Instantiate(_bullet, _firePoint.position, quaternion.identity,this.transform);
                _bullets.Add(bulls);
                NetworkServer.Spawn(bulls.gameObject);
                bulls.speed = _speed;
                bulls.damage = _damage;
                bulls.thisTR.position = _firePoint.position;
                bulls.thisTR.forward = _firePoint.forward + new Vector3 (Random.Range(-1*_scram,_scram), 0, Random.Range(-1*_scram,_scram));;
                bulls.previousePosition = _firePoint.position;
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

    public void FixedUpdate()
    {
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
                    DestroyBullet(_liveBullet[i]);
                    i--;
                }
            }
        }
    }
}