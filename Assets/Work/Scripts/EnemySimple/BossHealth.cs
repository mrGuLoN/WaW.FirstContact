using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BossHealth : AbstractHealth
{
    [SerializeField] private ParticleSystem _damage, _dead;
    private float _currentHealth = 100;
    public override void Awake()
    {
      EnemyController.instance.AddBoss();
    }

    // Update is called once per frame
    public override void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        CMDDamageEnemy(damage, point, direction);
    }
    
    [Server]

    public void CMDDamageEnemy(float damage, Vector3 point, Vector3 direction)
    {
       
        _currentHealth -= damage;
        var babas =Instantiate(_damage);
        NetworkServer.Spawn(babas.gameObject);
        DamageVision();
        if (_currentHealth <= 0)
        {
            EnemyController.instance.RemuveBoss();
            var baba =Instantiate(_dead);
            NetworkServer.Spawn(baba.gameObject);
           DeadVision();
        }
    }

    [ClientRpc]
    private void DamageVision()
    {
        var babas =Instantiate(_damage);
    }
    [ClientRpc]
    private void DeadVision()
    {
        var baba =Instantiate(_dead);
    }
    
   
}
