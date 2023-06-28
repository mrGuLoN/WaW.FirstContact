using Mirror;
using System.Collections.Generic;
using Enemy.StateMachine;
using Enemy.StateMachine.States;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    public List<Transform> playerList = new();
    [SerializeField] private List<EnemyControllerSM> _enemyControllerSmList = new();
    private List<EnemyControllerSM> _enemyDeadListSkin = new();

    public static EnemyController instance = null;

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

    public void AddPlayer(Transform player)
    {
        if (!isServer) return;
        playerList.Add(player);
    }

    public void AddEnemy(EnemyControllerSM enemyControllerSm)
    {
        if (!isServer) return;
        _enemyControllerSmList.Add(enemyControllerSm);
    }

    public void RemoveEnemy(EnemyControllerSM enemyControllerSm)
    {
        if (!isServer) return;
        _enemyControllerSmList.Remove(enemyControllerSm);
        _enemyDeadListSkin.Add(enemyControllerSm);
    }


    public void RemovePlayer(Transform player)
    {
        if (!isServer) return;
        playerList.Remove(player);
        for (int i = 0; i < _enemyControllerSmList.Count; i++)
        {
            _enemyControllerSmList[i].ChangeState(_enemyControllerSmList[i].idle);
        }
    }

    private void FixedUpdate()
    {
        if (!isServer) return;
        CallBackAll();
       
    }
    
    private void CallBackAll()
    {
        for (int i = 0; i < _enemyControllerSmList.Count; i++)
        {
            _enemyControllerSmList[i].UpdatePhysics();
        }

        for (int i = 0; i < _enemyDeadListSkin.Count; i++)
        {
            _enemyDeadListSkin[i].CheckDistanse();
        }
    }
}