using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ServerBulletController : NetworkBehaviour
{
    public static ServerBulletController instance = null;

    private List<AbstractBullet> _listAbstractBullets = new();

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

    public void AddBullet(AbstractBullet bullet)
    {
        _listAbstractBullets.Add(bullet);
    }

    private void Update()
    {
        if (_listAbstractBullets.Count > 0)
        {
            foreach (var bullet in _listAbstractBullets)
            {
                bullet.bulletTransform.position += bullet.bulletTransform.forward * bullet.bulletSpeed * Time.deltaTime;
            }
        }
    }
}