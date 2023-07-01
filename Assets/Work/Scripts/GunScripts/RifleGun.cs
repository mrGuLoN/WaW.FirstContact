using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RifleGun : AbstractGunScripts
{
    public override void SetAnimator(Animator Animator)
    {
        Debug.Log("RELOAD");
        animator = Animator;
    }
    
    
    public override void CmdFire()
    {
        _splash.Play();
        base.CmdFire();
    }
}
