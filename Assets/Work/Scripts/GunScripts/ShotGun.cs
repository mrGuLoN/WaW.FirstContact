using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : AbstractGunScripts
{
    // Start is called before the first frame update
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
