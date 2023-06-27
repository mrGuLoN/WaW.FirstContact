using System;
using System.Collections;
using Mirror;
using Player.StateMachine;
using UnityEngine;

public class AmmoScripts : NetworkBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        StartCoroutine(OnCollider());
    }

    IEnumerator OnCollider()
    {
        yield return new WaitForSeconds(3f);
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var abstractGunScripts = other.transform.gameObject.GetComponent<PlayerControllerSM>().firstGun;
        UpAmmo(abstractGunScripts);
        NetworkServer.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }

    
    private void UpAmmo(AbstractGunScripts gun)
    {
        gun.UpAmmo(10);
    }
}
