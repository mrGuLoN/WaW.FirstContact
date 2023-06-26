using System;
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
    [SerializeField] private int _magazine;
    [SerializeField] private int _allAmmo;
    [SerializeField] private AbstractBullet _bullet = new AbstractBullet();
    

    private Text _ammo;
    private int _currentMagazine;
    public void Start()
    {
        _ammo = CanvasController.instance.ammo;
        _currentMagazine = _magazine;
        _ammo.text = _currentMagazine + " / " + _allAmmo;
    }

    public virtual void SetAnimator(Animator Animator)
    {
        animator = Animator;
    }

    public virtual void CmdFire()
    {
        if (_currentMagazine <= 0)
        {
            animator.SetBool("Reload", true);
            return;
        }
        _currentMagazine--;
        _ammo.text = _currentMagazine + " / " + _allAmmo;
        for (int i = 0; i < _pelvis; i++)
        {
           SpawnBullet();
        }
    }

    public void Reloading()
    {
        animator.SetBool("Reload", false);
        if (_allAmmo > _magazine)
        {
            _currentMagazine = _magazine;
            _allAmmo -= _magazine;
            _ammo.text = _currentMagazine + " / " + _allAmmo;
        }
        else if (_allAmmo <= 0)
        {
            _currentMagazine = 1;
            _ammo.text = _currentMagazine + " / " + "Damn!!!";
        }
        else
        {
            _currentMagazine = _allAmmo;
            _allAmmo = 0;
            _ammo.text = _currentMagazine + " / " + "Damn!!!";
        }
    }


    public void SpawnBullet()
    {
        _bullet.damage = _damage;
        _bullet.speed = _speed;
        BulletController.instance.AddBullet(_bullet, _firePoint, _scram);
    }
}