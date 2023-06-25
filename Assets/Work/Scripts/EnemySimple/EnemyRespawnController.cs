using System.Collections;
using System.Collections.Generic;
using Enemy.StateMachine;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyRespawnController : NetworkBehaviour
{
    [SerializeField] private EnemyControllerSM _prefabEnemy;
    [SerializeField] private int _enemyCont;
    [SerializeField] private float _radiusRespawn;

    
    public void CmdEnemyRespawn()
    {
        for (int i = _enemyCont; i >= 0; i++)
        {
            var enemy = Instantiate(_prefabEnemy, this.transform.position + 
                                                  new Vector3(Random.Range(-1*_radiusRespawn,_radiusRespawn),0f,Random.Range(-1*_radiusRespawn,_radiusRespawn)), 
                quaternion.identity);
            NetworkServer.Spawn(enemy.gameObject);
        } 
    }

   
}
