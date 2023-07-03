using System;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AbstractGunScripts : MonoBehaviour
{
    public Animator animator;
    public BulletController bulletController;
    
    [SerializeField] protected ParticleSystem _splash;
    [SerializeField] protected AnimatorOverrideController _gunController;
    [SerializeField] private protected int _pelvis;
    [SerializeField] private protected float _damage;
    [SerializeField] private protected float _speed;
    [SerializeField] private protected float _scram;
    [SerializeField] public Transform _firePoint;
    [SerializeField] public int magazine;
    [SerializeField] public int allAmmo;
    [SerializeField] private AbstractBullet _bullet;

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
    }
    

    public virtual void SetAnimator(Animator Animator)
    {
        animator = Animator;
    }

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
            Fire();
        }
    }

    private void Fire()
    {
        bulletController.FireBullet(_damage, _speed , _scram);
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
}