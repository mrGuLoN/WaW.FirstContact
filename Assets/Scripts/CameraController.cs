using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [FormerlySerializedAs("_player")] public Transform player;
    private Transform _thisTR;
    
    void Start()
    {
        _thisTR = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _thisTR.position = new Vector3(player.position.x, _thisTR.position.y, player.position.z);
    }
}
