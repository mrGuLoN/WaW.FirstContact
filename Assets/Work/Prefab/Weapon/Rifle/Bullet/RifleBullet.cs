using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RifleBullet : AbstractBullet
{
    // Start is called before the first frame update
    void OnEnable()
    {
        TrailOff();
    }

   
  
    public override void TrailOn()
    {
        trailRenderer.SetActive(true);
        CmdTrailOn();
    }
    [Command(requiresAuthority = false)]
    public void CmdTrailOn()
    {
        trailRenderer.SetActive(true);
        RpcTrailOn();
    }

    [ClientRpc]
    private void RpcTrailOn()
    {
        trailRenderer.SetActive(true);
    }
   
    public override void TrailOff()
    {
        trailRenderer.SetActive(false);
        CmdTrailOff();
    }
    [Command(requiresAuthority = false)]

    public  void CmdTrailOff()
    {
        trailRenderer.SetActive(false);
        RpcTrailOff();
    }

    [ClientRpc]
    public  void RpcTrailOff()
    {
        trailRenderer.SetActive(false);

    }
}
