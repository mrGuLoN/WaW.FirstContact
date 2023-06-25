using Mirror;
using System.Collections.Generic;
using Enemy.StateMachine;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    public List<Transform> playerList = new();
    private List<EnemyControllerSM> _enemyControllerSmList = new();
    public static EnemyController instance = null;
    void Awake()
    {
        if (instance == null) 
        { 
            instance = this; 
        } 
        else if(instance == this)
        { 
            Destroy(gameObject);
        }
    }

    public void AddPlayer(Transform player)
    {
        if (isServer)
        playerList.Add(player);
    }

    public void AddEnemy(EnemyControllerSM enemyControllerSm)
    {
        if (isServer)
            _enemyControllerSmList.Add(enemyControllerSm);
    }
    public void RemoveEnemy(EnemyControllerSM enemyControllerSm)
    {
        if (isServer)
            _enemyControllerSmList.Remove(enemyControllerSm);
    }
    
    public void RemovePlayer(Transform player)
    {
        if (isServer)
        playerList.Remove(player);
    }
    private void FixedUpdate()
    {
        Debug.Log(_enemyControllerSmList.Count);
        for (int i = 0; i < _enemyControllerSmList.Count; i++)
        {
            _enemyControllerSmList[i].UpdatePhysics();
        }
    }
}
