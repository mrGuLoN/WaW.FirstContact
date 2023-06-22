using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RifleBullet : AbstractBullet
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
        
        base.Awake();
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1f);
        BulletController.instance.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
