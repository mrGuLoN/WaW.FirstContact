using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    [SerializeField] private List<AbstractBullet> _bullets = new List<AbstractBullet>();
    
    public static BulletController instance = null;
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

    public void AddBullet(AbstractBullet bullet)
    {
       if (isServer) _bullets.Add(bullet);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_bullets.Count > 0)
        {
            foreach (var bullet in _bullets)
            {
                bullet.previousePosition = bullet._thisTR.position;
                bullet._thisTR.position += bullet._thisTR.forward * bullet.speed * Time.deltaTime;
            }

            foreach (var bullet in _bullets)
            {
                float distance = Vector3.Distance(bullet.previousePosition, bullet._thisTR.position);
                RaycastHit hit;
                if (Physics.Raycast(bullet._thisTR.position, -1 * bullet._thisTR.forward, out hit, distance,
                        bullet.damageMask))
                {
                    hit.transform.GetComponent<AbstractDamageCollider>().TakeDamage(bullet.damage, hit.point, bullet._thisTR.forward, hit.transform.GetComponent<Rigidbody>());
                    _bullets.Remove(bullet);
                    NetworkServer.Destroy(bullet.gameObject);
                    Destroy(bullet.gameObject);
                }
            }
        }
    }
}
