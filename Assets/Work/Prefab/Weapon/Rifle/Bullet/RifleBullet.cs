using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RifleBullet : AbstractBullet
{
    // Start is called before the first frame update
    void OnEnable()
    {
        if (isServer)
        {
            gameObject.GetComponent<TrailRenderer>().enabled = false;
            return;
        }
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(15f);
        this.gameObject.SetActive(false);
        bulletController.DestroyBullet(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
