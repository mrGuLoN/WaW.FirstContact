using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    public List<Transform> playerList = new();
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
    
    public void RemovePlayer(Transform player)
    {
        if (isServer)
        playerList.Remove(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
