using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Destroyer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyThis());
    }

   IEnumerator DestroyThis()
   {
       yield return new WaitForSeconds(5f);
       NetworkServer.Destroy(this.gameObject);
       Destroy(this.gameObject);
   }

    // Update is called once per frame
    void Update()
    {
        
    }
}
