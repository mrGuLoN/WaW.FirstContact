using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleGun : AbstractGunScripts
{
    // Start is called before the first frame update
    public override void Fire()
    {
        _splash.Play();
        base.RespawnBullet(_firepointTransform);
    }
}
