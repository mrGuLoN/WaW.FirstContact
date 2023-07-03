using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Animator door;
    void Start()
    {
        EnemyController.instance.AddDoor(door);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
