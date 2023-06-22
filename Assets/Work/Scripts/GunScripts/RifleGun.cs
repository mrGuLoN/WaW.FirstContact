using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RifleGun : AbstractGunScripts
{
    
    public override void CmdFire()
    {
        _splash.Play();
        base.CmdFire();
    }
}
