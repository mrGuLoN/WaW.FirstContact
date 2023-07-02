using System.Collections.Generic;
using Mirror;
using Player.StateMachine;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    [SerializeField] private Transform firePoint, thisTR;
    [SerializeField] private AbstractBullet _bullet;
    [SerializeField] private LayerMask _damage;
    [SerializeField] private List<AbstractBullet> _sleepingBullet = new ();
    [SerializeField] private List<AbstractBullet> _wakeUpBullet = new();
    private Vector3 _previouseposition;
    

    void Start()
    {
        if (!isLocalPlayer) return;
        CmdRespawnBullet();
    }

    [Command(requiresAuthority = false)]
    private void CmdRespawnBullet()
    {
        for (int i = 0; i < 5; i++)
        {
            var bullet = Instantiate(_bullet, null);
            NetworkServer.Spawn(bullet.gameObject);
            _sleepingBullet.Add(bullet);
            bullet.TrailOff();
            BulletOff(bullet);
            AddToList(bullet);
        }
    }

    [ClientRpc]
    private void BulletOff(AbstractBullet bullet)
    {
        bullet.TrailOff();
    }
    [ClientRpc]
    private void AddToList(AbstractBullet bullet)
    {
        _sleepingBullet.Add(bullet);
        bullet.TrailOff();
    }

    public void FireBullet(float damage, float speed, float scram)
    {
        if (!isLocalPlayer) return;
        if (_sleepingBullet.Count <= 0)
        {
            CmdRespawnBullet();
        }
        CmdAddBullet(_sleepingBullet[0],damage,speed,scram);
        CmdRestartPosition(_sleepingBullet[0]);
    }

    [Command(requiresAuthority = false)]
    private void CmdAddBullet(AbstractBullet bullet,float damage, float speed, float scram)
    {
        _wakeUpBullet.Add(bullet);
        _sleepingBullet.Remove(bullet);
        bullet.damage = damage;
        bullet.speed = speed;
        bullet.thisTR.position = firePoint.position;
        bullet.thisTR.forward = thisTR.forward +
                                new Vector3(Random.Range(-1 * scram, scram), 0, Random.Range(-1 * scram, scram));
        RpcAddBullet(bullet, damage,  speed,  scram);
    }

    [ClientRpc]
    private void RpcAddBullet(AbstractBullet bullet,float damage, float speed, float scram)
    {
        _wakeUpBullet.Add(bullet);
        _sleepingBullet.Remove(bullet);
        bullet.damage = damage;
        bullet.speed = speed;
        bullet.thisTR.position = firePoint.position;
        bullet.thisTR.forward = thisTR.forward + new Vector3(Random.Range(-1*scram,scram),0,Random.Range(-1*scram,scram));
    }

    public void BulletFly()
    {
        if (!isLocalPlayer || _wakeUpBullet.Count <=0) return;
        for (int i = 0; i < _wakeUpBullet.Count; i++)
        {
            CmdTransformBullet(_wakeUpBullet[i]);
        }
        
    }

    [Command(requiresAuthority = false)]
    private void CmdTransformBullet(AbstractBullet bullet)
    {
        _previouseposition = bullet.thisTR.position;
        bullet.thisTR.position += bullet.thisTR.forward * bullet.speed * Time.deltaTime;
        CmdCheckWallEnemy(bullet, _previouseposition);
    }

    [Command(requiresAuthority = false)]
    private void CmdCheckWallEnemy(AbstractBullet bullet,Vector3 pPos)
    {
        Vector3 newPosition;
        var distance = Vector3.Distance(bullet.thisTR.position, pPos);
        RaycastHit hit;
        if (Physics.Raycast(bullet.thisTR.position, -1*bullet.thisTR.forward, out hit ,distance, _damage))
        {
            _sleepingBullet.Add(bullet);
            _wakeUpBullet.Remove(bullet);
            bullet.TrailOff();
            RpcReworkData(bullet);
            newPosition = hit.point;
            RpcTransformBullet(newPosition, bullet);
            Debug.Log(hit.transform.gameObject);
            AbstractDamageCollider plc;
            if (hit.transform.gameObject.TryGetComponent<AbstractDamageCollider>(out plc))
            {
                plc.TakeDamage(_bullet.damage,hit.point,bullet.thisTR.forward);
            }
        }
        else
        {
            newPosition = bullet.thisTR.position;
            RpcTransformBullet(newPosition, bullet);
        }
        
    }

    [ClientRpc]
    private void RpcTransformBullet(Vector3 newPosition,AbstractBullet bullet)
    {
        bullet.TrailOn();
        bullet.thisTR.position = newPosition;
    }


    [ClientRpc]
    private void RpcReworkData(AbstractBullet bullet)
    {
        _sleepingBullet.Add(bullet);
        _wakeUpBullet.Remove(bullet);
        bullet.TrailOff();
    }

    [Command(requiresAuthority = false)]
    private void CmdRestartPosition(AbstractBullet bullet)
    {
        bullet.thisTR.position = firePoint.position;
        bullet.thisTR.forward = thisTR.forward;
        RpcRestartPosition(bullet);
    }

    [ClientRpc]
    private void RpcRestartPosition(AbstractBullet bullet)
    {
        bullet.thisTR.position = firePoint.position;
        bullet.thisTR.forward = thisTR.forward;
    }
    
}