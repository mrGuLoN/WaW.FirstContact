using Enemy.StateMachine;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class Cell : NetworkBehaviour
{
    [SerializeField] private EnemyControllerSM _prefabEnemy;
    [SerializeField] private int _enemyCont;
    [SerializeField] private float _radiusRespawn;
    [SerializeField] private Transform _enemyRespawnController;
    [HideInInspector]
    public BoxCollider TriggerBox;
    public GameObject[] Exits;

    private void Awake()
    {
        TriggerBox = GetComponent<BoxCollider>();
        TriggerBox.isTrigger = true;
    }

    private void Start()
    {
        
    }
    
    public void CmdEnemyRespawn()
    {
        if(!isServer) return;
        for (int i = _enemyCont; i >= 0; i--)
        {
            var enemy = Instantiate(_prefabEnemy, Vector3.zero, quaternion.identity);
            NetworkServer.Spawn(enemy.gameObject);
            enemy.transform.position = _enemyRespawnController.position +
                                       new Vector3(Random.Range(-1 * _radiusRespawn, _radiusRespawn), 0f,
                                           Random.Range(-1 * _radiusRespawn, _radiusRespawn));
        } 
    }
}
