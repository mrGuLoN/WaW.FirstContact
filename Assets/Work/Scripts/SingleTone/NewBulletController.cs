using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewBulletController : NetworkBehaviour
{
    [SerializeField] private GameObject _bullet;

    [SerializeField] private List<AbstractBullet> _abstractBullets = new();
    [SerializeField] private List<AbstractBullet> _abstractBulletsLive = new();
    private float _distance;
    private Vector3 _newPosition;
    
    public static NewBulletController instance = null;

    public void Awake()
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
    void Start()
    {
        if (!isServer) return;
        for (int i = 20; i > 0; i--)
        {
            var buben =Instantiate(_bullet);
            _abstractBullets.Add(buben.GetComponent<AbstractBullet>());
            NetworkServer.Spawn(buben);
            buben.GetComponent<AbstractBullet>().trailRenderer.enabled = false;
            RpcStart(buben.GetComponent<AbstractBullet>());
        }
    }

    [ClientRpc]
    private void RpcStart(AbstractBullet bullet)
    {
        bullet.trailRenderer.enabled = false;
    }
    
    
    [Server]
    private void AddBulletServer(float speed, float damage, float scram, Transform firePoint)
    {
        for (int i =0; i < _abstractBullets.Count;i++)
        {
            if (_abstractBullets[i].trailRenderer.enabled == false)
            {
                _abstractBullets[i].speed = speed;
                _abstractBullets[i].damage = damage;
                _abstractBullets[i].thisTR.position = firePoint.position;
                _abstractBullets[i].thisTR.forward = firePoint.forward + new Vector3 (Random.Range(-1*scram,scram), 0, Random.Range(-1*scram,scram));
                _abstractBullets[i].previousePosition = firePoint.position;
                _abstractBulletsLive.Add(_abstractBullets[i]);
                _abstractBullets[i].trailRenderer.enabled = true;
                AddBulletPlayers(_abstractBullets[i]);
                return ;
            }
        }  
        var bulls = Instantiate(_bullet, firePoint.position, Quaternion.identity,this.transform);
        var bullsAbstract = bulls.GetComponent<AbstractBullet>(); 
        _abstractBullets.Add(bullsAbstract);
        NetworkServer.Spawn(bulls);
        bullsAbstract.speed = speed;
        bullsAbstract.damage = damage;
        bullsAbstract.thisTR.position = firePoint.position;
        bullsAbstract.thisTR.forward = firePoint.forward + new Vector3 (Random.Range(-1*scram,scram), 0, Random.Range(-1*scram,scram));
        bullsAbstract.previousePosition = firePoint.position;
        bullsAbstract.trailRenderer.enabled = true;
        _abstractBulletsLive.Add(bullsAbstract);
        _abstractBullets.Add(bullsAbstract);
        AddBulletPlayers(bullsAbstract);
    }

    [ClientRpc]
    private void AddBulletPlayers(AbstractBullet bullet)
    {
        bullet.trailRenderer.enabled = true;
    }

    public void AddBullet(float speed, float damage, float scram, Transform firePoint)
    {
        if (!isServer) return;
        AddBulletServer(speed, damage, scram, firePoint);
    }

    public void DestroyBullet(AbstractBullet bullet)
    {
        CmdDestroyBullet(bullet);
    }
    
   [Server]
    public void CmdDestroyBullet(AbstractBullet bullet)
    {
        _abstractBulletsLive.Remove(bullet);
        bullet.trailRenderer.enabled = false;
        RpcDestroyBullet(bullet);
    }

    [ClientRpc]
    private void RpcDestroyBullet(AbstractBullet bullet)
    {
        bullet.trailRenderer.enabled = false;
    }

    [ClientRpc]
    private void RpcBulletMove(Transform bullet, Vector3 newPosition)
    {
        bullet.position = newPosition;
    }

    private void FixedUpdate()
    {
        if (!isServer) return;
        for (int i = 0; i < _abstractBulletsLive.Count; i++)
        {
            _abstractBulletsLive[i].previousePosition = _abstractBulletsLive[i].thisTR.position;
            _abstractBulletsLive[i].thisTR.position += _abstractBulletsLive[i].thisTR.forward * _abstractBulletsLive[i].speed * Time.deltaTime;
            _newPosition = _abstractBulletsLive[i].thisTR.position;
           
        }

        for (int i = 0; i < _abstractBulletsLive.Count; i++)
        {
            _distance = Vector3.Distance(_abstractBulletsLive[i].thisTR.position, _abstractBulletsLive[i].previousePosition);
            RaycastHit hit;
            AbstractDamageCollider damageController;
            if (Physics.Raycast(_abstractBulletsLive[i].thisTR.position, -1 * _abstractBulletsLive[i].thisTR.forward, out hit, _distance,
                    _abstractBulletsLive[i].damageMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out damageController))
                    damageController.TakeDamage(_abstractBulletsLive[i].damage,hit.point,_abstractBulletsLive[i].thisTR.forward);
                RpcBulletMove( _abstractBulletsLive[i].thisTR, hit.point);
                CmdDestroyBullet(_abstractBulletsLive[i]);
                i--;
            }
            else
            {
                RpcBulletMove( _abstractBulletsLive[i].thisTR, _newPosition);
            }
        }
    }
}
